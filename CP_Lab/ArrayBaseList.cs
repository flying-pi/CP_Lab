using System;
using System.Collections;
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
        private int _version;

        public ArrayBaseList()
        {
            _items = new T[0];
            _realSize = 0;
            _version = 0;
        }

        public int Count => _realSize;

        public void Add(T product)
        {
            modifyArrayForNewElements();
            _items[_realSize] = product;
            _realSize++;
            _version++;
        }

        public void AddAll(IList<T> items)
        {
            int offsetStart = _realSize;
            modifyArrayForNewElements(items.Count);
            for (int i = 0; i < items.Count; i++)
                _items[offsetStart++] = items[i];
            _realSize += items.Count;
            _version++;
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
            set
            {
                _items[i] = value;
                _version++;
            }
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
            _version++;
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
            Sort((first, second) => first.CompareTo(second));
        }

        public void Sort(CompareDeligate<T> compare)
        {
            string buf;
            for (int i = 0; i < _realSize - 1; i++)
            {
                if (compare(_items[i], _items[i + 1]) > 0)
                {
                    buf = _items[i].Name;
                    _items[i].Name = _items[i + 1].Name;
                    _items[i + 1].Name = buf;
                    if (i > 0)
                        i -= 2;
                }
            }
            _version++;
        }

        public IEnumerable<T> GetReverseEnumerator()
        {
            int validV = _version;
            for (int i = _realSize - 1; i >= 0; i--)
            {
                if (validV != _version)
                    throw new ChangeListInForeachException();
                yield return _items[i];
            }
        }

        public T find(FindDeligate<T> checker)
        {
            for (int i = 0; i < _realSize; i++)
            {
                if (checker(_items[i]))
                    return _items[i];
            }
            return default(T);
        }

        public ICollection<T> findAll(FindDeligate<T> checker)
        {
            ArrayBaseList<T> result = new ArrayBaseList<T>();
            
            for (int i = 0; i < _realSize; i++)
            {
                if (checker(_items[i]))
                    result.Add(_items[i]);
            }
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int validV = _version;
            for (int i = 0; i < _realSize; i++)
            {
                if (validV != _version)
                    throw new ChangeListInForeachException();
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}