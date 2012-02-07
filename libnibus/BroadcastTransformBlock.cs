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
        }

        #endregion //Constructors

        #region Implementation of IDataflowBlock

        /// <summary>
        /// Signals to the <see cref="IDataflowBlock"/> that it should not accept nor produce any more messages
        /// nor consume any more postponed messages. 
        /// </summary>
        /// <seealso cref="IDataflowBlock"/>
        public void Complete()
        {
            _target.Complete();
        }

        /// <summary>
        /// Causes the <see cref="IDataflowBlock"/> to complete in a Faulted state. 
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> that caused the faulting.</param>
        /// <seealso cref="IDataflowBlock"/>
        public void Fault(Exception exception)
        {
            _target.Fault(exception);
        }

        /// <summary>
        /// Gets a <see cref="Task"/> that represents the asynchronous operation and completion of the dataflow block.
        /// </summary>
        /// <seealso cref="IDataflowBlock"/>
        public Task Completion
        {
            get { return _source.Completion; }
        }

        #endregion

        #region Implementation of ISourceBlock<out TOutput>

        /// <summary>
        /// Links the ISourceBlock`1 to the specified ITargetBlock`1. 
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="unlinkAfterOne">if set to <c>true</c> [unlink after one].</param>
        /// <returns></returns>
        public IDisposable LinkTo(ITargetBlock<TOutput> target, bool unlinkAfterOne)
        {
            return _source.LinkTo(target, unlinkAfterOne);
        }

        /// <summary>
        /// Called by a linked <see cref="ITargetBlock{TOutput}"/> to accept and consume
        /// a DataflowMessageHeader previously offered by this <see cref="ISourceBlock{TInput}"/>.
        /// </summary>
        /// <param name="messageHeader">The <see cref="DataflowMessageHeader"/> of the message being consumed.</param>
        /// <param name="target">The <see cref="ITargetBlock{TOutput}"/> consuming the message. </param>
        /// <param name="messageConsumed"><c>True</c> if the message was successfully consumed. <c>False</c> otherwise. </param>
        /// <returns>
        /// The value of the consumed message. This may correspond to a different DataflowMessageHeader
        /// instance than was previously reserved and passed as the messageHeader to ConsumeMessage.
        /// The consuming ITargetBlock`1 must use the returned value instead of the value passed as messageValue
        /// through OfferMessage.
        /// </returns>
        /// <seealso cref="ISourceBlock{TOutput}"/>
        public TOutput ConsumeMessage(
            DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed)
        {
            return _source.ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        /// <summary>
        /// Called by a linked <see cref="ITargetBlock{TOutput}"/> to reserve a previously offered
        /// <see cref="DataflowMessageHeader"/> by this <see cref="ISourceBlock{TInput}"/>.
        /// </summary>
        /// <param name="messageHeader">The <see cref="DataflowMessageHeader"/> of the message being consumed.</param>
        /// <param name="target">The <see cref="ITargetBlock{TOutput}"/> consuming the message. </param>
        /// <returns><c>true</c> if the message was successfully reserved; otherwise, <c>false</c>.</returns>
        /// <seealso cref="ISourceBlock{TOutput}"/>
        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            return _source.ReserveMessage(messageHeader, target);
        }

        /// <summary>
        /// Called by a linked <see cref="ITargetBlock{TOutput}"/> to release a previously reserved
        /// <see cref="DataflowMessageHeader"/> by this <see cref="ISourceBlock{TInput}"/>
        /// </summary>
        /// <param name="messageHeader">The <see cref="DataflowMessageHeader"/> of the message being consumed.</param>
        /// <param name="target">The <see cref="ITargetBlock{TOutput}"/> consuming the message. </param>
        /// <seealso cref="ISourceBlock{TOutput}"/>
        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target)
        {
            _source.ReleaseReservation(messageHeader, target);
        }

        #endregion

        #region Implementation of IReceivableSourceBlock<TOutput>

        /// <summary>
        /// Attempts to synchronously receive an available output item from the <see cref="ISourceBlock{TInput}"/>. 
        /// </summary>
        /// <param name="filter">The predicate a value must successfully pass in order for it to be received. filter may be <c>null</c>, in which case all items will pass.</param>
        /// <param name="item">The item received from the source.</param>
        /// <returns><c>true</c> if an item could be received; otherwise, <c>false</c>.</returns>
        /// <seealso cref="IReceivableSourceBlock{TOutput}"/>
        public bool TryReceive(Predicate<TOutput> filter, out TOutput item)
        {
            return _source.TryReceive(filter, out item);
        }

        /// <summary>
        /// Attempts to synchronously receive all available items from the <see cref="ISourceBlock{TInput}"/>. 
        /// </summary>
        /// <param name="items">The items received from the source.</param>
        /// <returns><c>true</c> if one or more items could be received; otherwise, <c>false</c>.</returns>
        /// <seealso cref="IReceivableSourceBlock{TOutput}"/>
        public bool TryReceiveAll(out IList<TOutput> items)
        {
            return _source.TryReceiveAll(out items);
        }

        #endregion

        #region Implementation of ITargetBlock

        public DataflowMessageStatus OfferMessage(
            DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept)
        {
            return _target.OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }

        #endregion

        /// <summary>
        /// Возвращает источник, который нужно использовать вместо себя при использовании фильтра.
        /// </summary>
        public IReceivableSourceBlock<TOutput> Source
        {
            get { return _source; }
        }
    }
}