using MVVM;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;

namespace MVVM.Interfaces
{
    public interface IFactory<out T>
    {
        T Create();
    }

    public interface IFactory<in TArg, out TOut>
    {
        TOut Create(TArg arg);
    }
}
