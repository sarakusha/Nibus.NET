//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NibusStack.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace NataInfo.Nibus
{
    public class NibusStack : IDisposable
    {
        #region Member Variables

        private readonly HashSet<ICodecInfo> _codecs;

        #endregion

        #region Constructors

        public NibusStack(params ICodecInfo[] codecs)
        {
            _codecs = new HashSet<ICodecInfo>();
            if (codecs.Length > 0)
            {
                AddChain(codecs);
            }
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties

        #region Methods

        public void AddChain(params ICodecInfo[] codecs)
        {
            if (codecs.Length < 2)
            {
                throw new ArgumentException("Must be at least two codecs");
            }

            var links = new List<IDisposable>(codecs.Length - 1);
            for (var i = codecs.Length - 1; i > 0; i--)
            {
                var link = DynamicConnectTo(codecs[i], codecs[i - 1]);
                if (link == null)
                {
                    links.ForEach(l => l.Dispose());
                    throw new ArgumentException("Invalid codec type");
                }

                links.Add(link);
            }

            codecs.ToList().ForEach(codec => _codecs.Add(codec));
        }

        public IEnumerable<T> GetCodecs<T>()
        {
            return _codecs.OfType<T>();
        }

        public T GetCodec<T>()
        {
            return _codecs.OfType<T>().Single();
        }

        #endregion //Methods

        #region Implementations

        private static IDisposable DynamicConnectTo(object topCodec, object bottomCodec)
        {
            var bottomCodecType = bottomCodec.GetType();
            foreach (MethodInfo mi in topCodec.GetType().GetMember(
                        "ConnectTo", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public))
            {
                var pi = mi.GetParameters().Single();
                var it = bottomCodecType.GetInterface(pi.ParameterType.Name);
                if (it != null)
                {
                    var connectTo = mi;
                    if (mi.IsGenericMethod)
                    {
                        var genTypes = it.GetGenericArguments();
                        connectTo = mi.MakeGenericMethod(genTypes.First());
                    }

                    return (IDisposable)connectTo.Invoke(topCodec, new[] { bottomCodec });
                }
            }

            return null;
        }

        #endregion //Implementations

        #region Implementation of IDisposable

        public void Dispose()
        {
            _codecs.Cast<IDisposable>().ToList().ForEach(codec => codec.Dispose());
        }

        #endregion
    }
}