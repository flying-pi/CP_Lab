using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CP_Lab
{
    // creating custom delegate because i don`t know yout opinion about standard Action and Func delegate
    public delegate bool FindDeligate<T>(T item) where T : IProduct;

    public delegate int CompareDeligate<T>(T first, T second) where T : IProduct;

    public interface ICollection<T> : IEnumerable<T>, IEnumerable where T : IProduct
    {
        int Count { get; }
        void Add(T product);
        void AddAll(IList<T> items);
        T this[int i] { get; set; }
        T this[string name] { get; }
        void RemoveAt(int position);
        void RemoveByName(string name);
        void Remove(T original);
        void Sort();
        void Sort(CompareDeligate<T> compare);
        IEnumerable<T> GetReverseEnumerator();
        T find(FindDeligate<T> checker);
        ICollection<T> findAll(FindDeligate<T> checker);
        

    }


    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() : this("Can not found product with given name")
        {
        }

        public KeyNotFoundException(string message) : base(message)
        {
        }

        protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public KeyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : this("can not found given item in collection")
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }

        protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ChangeListInForeachException : Exception
    {
        public ChangeListInForeachException() : this("list was changed when foreach operator work")
        {
        }

        public ChangeListInForeachException(string message) : base(message)
        {
        }

        protected ChangeListInForeachException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ChangeListInForeachException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}