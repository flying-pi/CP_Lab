using System;
using System.Collections;
using System.Collections.Generic;

namespace CP_Lab
{
    public class LinkedList<T> : ICollection<T> where T : IProduct
    {
        private int size = -1;
        public int Count => size + 1;
        private int _version = 0;
        private ListItem<T> _first;
        private ListItem<T> _last;

        public LinkedList()
        {
        }

        public void Add(T product)
        {
            ListItem<T> newItem = new ListItem<T>(product);
            newItem.Previuse = _last;
            if (_last != null)
                _last.Next = newItem;
            if (_first == null)
                _first = _last;
            _last = newItem;
            size++;
            _version++;
        }

        public void AddAll(IList<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        T ICollection<T>.this[int i]
        {
            get { return GetNodeByPosition(i).Vale; }
            set
            {
                GetNodeByPosition(i).Vale = value;
                _version++;
            }
        }

        private ListItem<T> GetNodeByPosition(int pos)
        {
            if (pos < 0 || pos > size)
                throw new IndexOutOfRangeException();
            ListItem<T> cursor = _first;
            for (int i = 0; i < pos; i++)
                cursor = cursor.Next;
            return cursor;
        }

        T ICollection<T>.this[string name] => getNodeByName(name).Vale;

        private ListItem<T> getNodeByName(string name)
        {
            ListItem<T> cursor = _first;
            while (cursor != null)
            {
                if (cursor.Vale.Name.Equals(name))
                    return cursor;
                cursor = cursor.Next;
            }
            throw new KeyNotFoundException();
        }

        protected void removeItem(ListItem<T> item)
        {
            if (item.Previuse == null)
            {
                _first = _first.Next;
                return;
            }
            item.Previuse.Next = item.Next;
            item.Next.Previuse = item.Previuse;
            _version++;
        }

        public void RemoveAt(int position)
        {
            removeItem(GetNodeByPosition(position));
        }

        public void RemoveByName(string name)
        {
            removeItem(getNodeByName(name));
        }

        public void Remove(T original)
        {
            ListItem<T> cursor = _first;
            while (cursor != null)
            {
                if (cursor.Vale.Equals(original))
                {
                    removeItem(cursor);
                    return;
                }
                cursor = cursor.Next;
            }
            throw new ItemNotFoundException();
        }

        public void Sort()
        {
            Sort((first, second) => first.CompareTo(second));
        }

        public void Sort(CompareDeligate<T> compare)
        {
            ListItem<T> cursor = _first;
            while (cursor != null && cursor.Next != null)
            {
                if (compare(cursor.Vale, cursor.Next.Vale) > 0)
                {
                    T temp = cursor.Vale;
                    cursor.Vale = cursor.Next.Vale;
                    cursor.Next.Vale = temp;
                    if (cursor.Previuse != null)
                        cursor = cursor.Previuse;
                    else
                        cursor = cursor.Next;
                }
                else
                {
                    cursor = cursor.Next;
                }
            }
            _version++;
        }

        public IEnumerable<T> GetReverseEnumerator()
        {
            ListItem<T> cursor = _last;
            int validV = _version;
            while (cursor != null)
            {
                if (validV != _version)
                    throw new ChangeListInForeachException();
                yield return cursor.Vale;
                cursor = cursor.Previuse;
            }
        }

        public T find(FindDeligate<T> checker)
        {
            ListItem<T> cursor = _first;
            while (cursor != null)
            {
                if (checker(cursor.Vale))
                    return cursor.Vale;
                cursor = cursor.Next;
            }
            return default(T);
        }

        public ICollection<T> findAll(FindDeligate<T> checker)
        {
            LinkedList<T> result = new LinkedList<T>();
            ListItem<T> cursor = _first;
            while (cursor != null)
            {
                if (checker(cursor.Vale))
                    result.Add(cursor.Vale);
                cursor = cursor.Next;
            }
            return result;
        }


        public IEnumerator<T> GetEnumerator()
        {
            ListItem<T> cursor = _first;
            int validV = _version;
            while (cursor != null)
            {
                if (validV != _version)
                    throw new ChangeListInForeachException();
                yield return cursor.Vale;
                cursor = cursor.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ListItem<T>
    {
        public ListItem(T product)
        {
            Vale = product;
        }

        public ListItem<T> Next { get; set; }
        public ListItem<T> Previuse { get; set; }
        public T Vale { get; set; }
    }
}