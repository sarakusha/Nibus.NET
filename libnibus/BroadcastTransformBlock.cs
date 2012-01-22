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
    /// <summary>
    /// Переобразующий широковещательный блок.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <seealso cref="TransformBlock{TInput,TOutput}"/>
    /// <seealso cref="BroadcastBlock{T}"/>
    public sealed class BroadcastTransformBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>,
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
        /// <param name="transform">Функция трансформации.</param>
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

        public TOutput ConsumeMessage(
            DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
        {
            return _source.ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            return _source.ReserveMessage(messageHeader, target);
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
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

        public DataflowMessageStatus OfferMessage(
            DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
        {
            return _target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        /// <summary>
        /// Возвращает источник, необходимый при испольовании фильтра.
        /// </summary>
        public IReceivableSourceBlock<TOutput> Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Возвращает действие для сброса выходного источника заданным значением.
        /// </summary>
        public Action<TOutput> ResetSource { get; private set; }
    }
}