using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CP_Lab.test
{
    
    [TestFixture(typeof(ArrayBaseList<IProduct>))]
    [TestFixture(typeof(LinkedList<IProduct>))]
    public class StructureTest<T> where T : ICollection<IProduct>, new()
    {
        private T _collection;

        [SetUp]
        public void BeforTest()
        {
            _collection = new T();
        }

        [TearDown]
        public void AfterTest()
        {
            _collection = default(T);
        }

        private static List<T> GetItemList<T>(int size = 10, string namePrefix = "testProduct")
            where T : IProduct, new()
        {
            List<T> result = new List<T>();
            for (int i = 0; i < size; i++)
                result.Add(new T {Name = namePrefix + i});
            return result;
        }

        [Test]
        public void AddTest()
        {
            List<Drama> addedProduct = GetItemList<Drama>();
            foreach (Drama i in addedProduct)
                _collection.Add(i);
            Assert.AreEqual(addedProduct.Count, _collection.Count);
            for (int i = 1; i < addedProduct.Count; i++)
                Assert.AreSame(_collection[i], addedProduct[i]);
        }


        [Test]
        public void AllAddTest()
        {
            List<IProduct> addedProduct = new List<IProduct>(GetItemList<Drama>());
            _collection.AddAll(addedProduct);
            Assert.AreEqual(addedProduct.Count, _collection.Count);
            for (int i = 1; i < addedProduct.Count; i++)
                Assert.AreSame(_collection[i], addedProduct[i]);
        }

        [Test]
        public void IndexCheckTetst()
        {
            string namePrefix = "t";
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(10, namePrefix));
            _collection.AddAll(itemList);
            for (int i = 0; i < itemList.Count; i++)
                Assert.AreSame(_collection[namePrefix + i], itemList[i]);
        }

        private static void deleteTestHelper(List<IProduct> itemList, T testedCollection,
            Action<int, List<IProduct>, T> deleteAction)
        {
            testedCollection.AddAll(itemList);
            Random random = new Random();
            int r;
            for (int i = 0; i < itemList.Count / 5; i++)
            {
                r = random.Next(0, itemList.Count - 1);
                deleteAction(r, itemList, testedCollection);
            }
            for (int i = 0; i < itemList.Count; i++)
                Assert.AreSame(testedCollection[i], itemList[i]);
        }

        [Test]
        public void DeleteByIndexTest()
        {
            
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100));
            deleteTestHelper(itemList, _collection, (position, originalList, testCollect) =>
            {
                originalList.RemoveAt(position);
                testCollect.RemoveAt(position);
            });
            
            Assert.Throws<IndexOutOfRangeException>(() => _collection.RemoveAt(100));
        }

        [Test]
        public void DeleteByNameTest()
        {
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100,namePrefix:"del"));
            deleteTestHelper(itemList, _collection, (position, originalList, testCollect) =>
            {
                testCollect.RemoveByName(originalList[position].Name);
                originalList.RemoveAt(position);
            });
            
            Assert.Throws<KeyNotFoundException>(() => _collection.RemoveByName("unknowName"));
        }

        [Test]
        public void DeleteByItemTest()
        {
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100,namePrefix:"del"));
            deleteTestHelper(itemList, _collection, (position, originalList, testCollect) =>
            {
                testCollect.Remove(originalList[position]);
                originalList.RemoveAt(position);
            });
            
            Assert.Throws<KeyNotFoundException>(() => _collection.RemoveByName("unknowName"));
        }

        [Test]
        public void SortTest()
        {
            string[] names = {"mock","cort","bor","artica"};
            foreach (string name in names)
            {
                _collection.Add(new Drama(name:name));
            }
            _collection.Sort();

            for (int i = 0; i < names.Length; i++)
            {
                Assert.AreEqual(names[names.Length-1-i],_collection[i].Name);
            }
        }

        [Test]
        public void IteratorTest()
        { 
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>());
            _collection.AddAll(itemList);

            int i = 0;
            foreach (IProduct product in _collection)
            {
                Assert.AreSame(itemList[i++],product);
            }
        }
        [Test]
        public void ReversIteratorTest()
        { 
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>());
            _collection.AddAll(itemList);

            int i = itemList.Count-1;
            foreach (IProduct product in _collection.GetReverseEnumerator())
            {
                Assert.AreSame(itemList[i--],product);
            }
        }

        public static void EnumitatorTestHelper(ICollection<IProduct> collection,IEnumerable<IProduct> iterator)
        {
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.Remove(collection[0]);
                }
            });
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.RemoveByName(collection[0].Name);
                }
            });
            
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.RemoveAt(0);
                }
            });
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.Sort();
                }
            });
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection[0] = new Drama();
                }
            });
            
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.Add(new Drama());
                }
            });
            
            Assert.Throws<ChangeListInForeachException>(() =>
            {
                foreach (var v in iterator)
                {
                    collection.AddAll( new List<IProduct>(GetItemList<Drama>()));
                }
            });
        }

        [Test]
        public void IteratorChangeTest()
        {
            _collection.AddAll( new List<IProduct>(GetItemList<Drama>(10)));
            EnumitatorTestHelper(_collection, _collection);
        }
        [Test]
        public void IteratorReverceChangeTest()
        {
            _collection.AddAll( new List<IProduct>(GetItemList<Drama>(10)));
            EnumitatorTestHelper(_collection, _collection.GetReverseEnumerator());
        }
    }
}