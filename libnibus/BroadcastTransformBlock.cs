//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// BroadcastTransformBlock.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

#endregion

namespace NataInfo.Nibus
{
    public class BroadcastTransformBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>,
                                                            IReceivableSourceBlock<TOutput>
    {
        #region Member Variables

        private readonly IReceivableSourceBlock<TOutput> _source;
        private readonly ITargetBlock<TInput> _target;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public BroadcastTransformBlock(Func<TInput, TOutput> transform)
        {
            var source = new BroadcastBlock<TOutput>(m => m);
            var target = new ActionBlock<TInput>(m => source.Post(transform(m)));
            target.Completion.ContinueWith(delegate { source.Complete(); });
            _target = target;
            _source = source;
            ResetSource = item => source.Post(item);
        }

        #endregion //Constructors

        #region Implementation of IDataflowBlock

        public void Complete()
        {
            _target.Complete();
        }

        public void Fault(Exception exception)
        {
            _target.Fault(exception);
        }

        public Task Completion
        {
            get { return _source.Completion; }
        }

        #endregion

        #region Implementation of ISourceBlock<out TOutput>

        public IDisposable LinkTo(ITargetBlock<TOutput> target, bool unlinkAfterOne)
        {
            return _source.LinkTo(target, unlinkAfterOne);
        }

        TOutput ISourceBlock<TOutput>.ConsumeMessage(
            DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
        {
            return _source.ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            return _source.ReserveMessage(messageHeader, target);
        }

        void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            _source.ReleaseReservation(messageHeader, target);
        }

        #endregion

        #region Implementation of IReceivableSourceBlock<TOutput>

        public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
        {
            return _source.TryReceive(filter, out item);
        }

        public bool TryReceiveAll(out IList<TOutput> items)
        {
            return _source.TryReceiveAll(out items);
        }

        #endregion

        DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(
            DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
        {
            return _target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        public IReceivableSourceBlock<TOutput> Source
        {
            get { return _source; }
        }

        public Action<TOutput> ResetSource { get; private set; }
    }
}