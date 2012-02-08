//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// ReadProgressInfo.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Threading.Tasks;
using NataInfo.Nibus.Nms.Services;

#endregion

namespace NataInfo.Nibus.Nms
{
    /// <summary>
    /// Содержит информацию о прогрессе получения запрашиваемых переменных при пакетном запросе <see cref="NmsProtocol.ReadManyValuesAsync"/>.
    /// </summary>
    /// <seealso cref="NibusOptions"/>
    /// <example>
    /// <code>
    /// var options = new NibusOptions { Progress = new Progress&lt;object&gt;(o => Console.WriteLine(((ReadProgressInfo)o).Value)) };
    /// await NmsProtocol.ReadManyValuesAsync(options, address, (int)StdNms.SoftwareId, (int)StdNms.SoftwareVersion);
    /// </code>
    /// </example>
    public sealed class ReadProgressInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadProgressInfo"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="id">Идентификатор переменной.</param>
        /// <param name="task">Завершенная асинхронная задача чтения переменной.</param>
        internal ReadProgressInfo(Address source, int id, Task<NmsRead> task)
        {
            Id = id;
            Source = source;
            if (task.Exception != null)
            {
                Exception = task.Exception.Flatten().InnerException;
            }

            if (task.IsFaulted)
            {
                IsFaulted = true;
            }
            else if (task.IsCanceled)
            {
                IsCanceled = true;
            }
            else
            {
                var nmsRead = task.Result;
                Value = nmsRead.Value;
                ValueType = nmsRead.ValueType;
            }
        }

        /// <summary>
        /// Возвращает адрес источника.
        /// </summary>
        public Address Source { get; private set; }

        /// <summary>
        /// Возвращает идентификатор переменной.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Возвращает тип переменной.
        /// </summary>
        public NmsValueType ValueType { get; private set; }

        /// <summary>
        /// Возвращает значение переменной.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Сигнализирует, что операция была отменена пользователем.
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// Сигнализирует, что опереация чтения не удалась. Исключение сохранено в <see cref="Exception"/>.
        /// </summary>
        public bool IsFaulted { get; private set; }

        /// <summary>
        /// Возвращает исключение полученное в случае неудачной операции чтения переменной.
        /// </summary>
        public Exception Exception { get; private set; }

        #region Constructors

        //public ReadProgressInfo(Address source, int id, Exception e, NmsRead nmsRead)
        //{
        //    Id = id;
        //    Source = source;
        //    if (e != null)
        //    {
        //        Exception = e;
        //        if (e is TaskCanceledException)
        //        {
        //            IsCanceled = true;
        //        }
        //        else
        //        {
        //            IsFaulted = true;
        //        }
        //    }
        //    else
        //    {
        //        Value = nmsRead.Value;
        //        ValueType = nmsRead.ValueType;
        //    }
        //}

        #endregion //Constructors
    }
}