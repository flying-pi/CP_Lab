using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CP_Lab
{
    public interface ICollection<T> where T:IProduct
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
        public ItemNotFoundException():this("can not found given item in collection")
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
}