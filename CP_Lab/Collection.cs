using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace CP_Lab
{
    public class Collection
    {
        private IProduct[] _items;
        private int _realSize;

        public Collection()
        {
            _items = new IProduct[0];
            _realSize = 0;
        }

        public int Count => _realSize;

        public void Add(IProduct product)
        {
            modifyArrayForNewElements();
            _items[_realSize] = product;
            _realSize++;
        }

        public void AddAll(IList<IProduct> items)
        {
            int offsetStart = _realSize;
            modifyArrayForNewElements(items.Count);
            for(int i=0;i<items.Count;i++)
                _items[offsetStart++] = items[i];
            _realSize += items.Count;
        }

        private void modifyArrayForNewElements(int newItemsCount = 1)
        {
            if (_realSize + newItemsCount > _items.Length)
            {
                IProduct[] buf = new IProduct[_realSize + newItemsCount];
                for (int i = 0; i < _realSize; i++)
                    buf[i] = _items[i];
                _items = buf;
            }
        }

        public IProduct this[int i]
        {
            get { return _items[i]; }
            set { _items[i] = value; }
        }

        public IProduct this[string name]
        {
            get
            {
                foreach (IProduct i in _items)
                    if (i.Name == name)
                        return i;
                throw new KeyNotFoundException();
            }
        }


        public void RemoveAt(int position)
        {
            if(position < 0 || position > _realSize)
                throw new IndexOutOfRangeException();
            _realSize--;
            if (position >= _realSize)
                return;
            for (int i = position; i < _realSize; i++)
                _items[i] = _items[i + 1];

        }

        public bool RemoveFirst(Object initState,Func<Object,IProduct,bool> checker)
        {
            int i = 0;
            for(i=0;i<_realSize;i++)
                if (checker(initState,_items[i]))
                    break;
            if (i == _realSize)
                return false;
            RemoveAt(i);
            return true;
        }

        /// <summary>
        /// usses for check is item has given name
        /// in  <seealso cref="RemoveFirst(Object,Func)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool nameChecker(Object name, IProduct item)
        {
            return item.Name.Equals(name);
        }

        public void RemoveByName(string name)
        {       
            int i = 0;
            for(i=0;i<_realSize;i++)
                if (_items[i].Name == name)
                    break;
            if (i == _realSize)
                throw new KeyNotFoundException();
            RemoveAt(i);
        }

        public void Remove(IProduct original)
        {
            int i = 0;
            for(i=0;i<_realSize;i++)
                if (_items[i].Equals(original))
                    break;
            if (i == _realSize)
                throw new KeyNotFoundException();
            RemoveAt(i);
        }

        public void Sort()
        {
            string buf;
            for (int i = 0; i < _realSize-1; i++)
            {
                if (_items[i].CompareTo(_items[i + 1])>0)
                {
                    buf = _items[i].Name;
                    _items[i].Name = _items[i + 1].Name;
                    _items[i + 1].Name = buf;
                    if (i > 0)
                        i -= 2;
                }
            }
        }
    }

    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException():this("Can not found product with given name")
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
}