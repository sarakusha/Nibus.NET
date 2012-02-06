//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// ReadProgressInfo.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    public sealed class ReadProgressInfo
    {
        public Address Source { get; private set; }
        public int Id { get; private set; }
        public NmsValueType ValueType { get; private set; }
        public object Value { get; private set; }
        public bool IsCanceled { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Exception { get; private set; }

        #region Constructors


        public ReadProgressInfo(Address source, int id, Task<NmsRead> task)
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

        public ReadProgressInfo(Address source, int id, Exception e, NmsRead nmsRead)
        {
            Id = id;
            Source = source;
            if (e != null)
            {
                Exception = e;
                if (e is TaskCanceledException)
                {
                    IsCanceled = true;
                }
                else
                {
                    IsFaulted = true;
                }
            }
            else
            {
                Value = nmsRead.Value;
                ValueType = nmsRead.ValueType;
            }
        }

        #endregion //Constructors
    }
}
