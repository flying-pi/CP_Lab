using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace CP_Lab
{
    public class ArrayBaseList<T> : ICollection<T> where T : IProduct
    {
        private T[] _items;
        private int _realSize;

        public ArrayBaseList()
        {
            _items = new T[0];
            _realSize = 0;
        }

        public int Count => _realSize;

        public void Add(T product)
        {
            modifyArrayForNewElements();
            _items[_realSize] = product;
            _realSize++;
        }

        public void AddAll(IList<T> items)
        {
            int offsetStart = _realSize;
            modifyArrayForNewElements(items.Count);
            for (int i = 0; i < items.Count; i++)
                _items[offsetStart++] = items[i];
            _realSize += items.Count;
        }

        private void modifyArrayForNewElements(int newItemsCount = 1)
        {
            if (_realSize + newItemsCount > _items.Length)
            {
                T[] buf = new T[_realSize + newItemsCount];
                for (int i = 0; i < _realSize; i++)
                    buf[i] = _items[i];
                _items = buf;
            }
        }

        public T this[int i]
        {
            get { return _items[i]; }
            set { _items[i] = value; }
        }

        public T this[string name]
        {
            get
            {
                foreach (T i in _items)
                    if (i.Name == name)
                        return i;
                throw new KeyNotFoundException();
            }
        }


        public void RemoveAt(int position)
        {
            if (position < 0 || position > _realSize)
                throw new IndexOutOfRangeException();
            _realSize--;
            if (position >= _realSize)
                return;
            for (int i = position; i < _realSize; i++)
                _items[i] = _items[i + 1];
        }

        public void RemoveByName(string name)
        {
            
            int i = 0;
            for (i = 0; i < _realSize; i++)
                if (_items[i].Name == name)
                    break;
            if (i == _realSize)
                throw new KeyNotFoundException();
            RemoveAt(i);
        }

        public void Remove(T original)
        {
            int i = 0;
            for (i = 0; i < _realSize; i++)
                if (_items[i].Equals(original))
                    break;
            if (i == _realSize)
                throw new ItemNotFoundException();
            RemoveAt(i);
        }

        public void Sort()
        {
            string buf;
            for (int i = 0; i < _realSize - 1; i++)
            {
                if (_items[i].CompareTo(_items[i + 1]) > 0)
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
}