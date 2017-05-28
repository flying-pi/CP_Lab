using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CP_Lab.test
{
    [TestFixture]
    public class StructureTest
    {
        private Collection testCollection;

        [SetUp]
        public void BeforTest()
        {
            testCollection = new Collection();
        }

        [TearDown]
        public void AfterTest()
        {
            testCollection = null;
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
                testCollection.Add(i);
            Assert.AreEqual(addedProduct.Count, testCollection.Count);
            for (int i = 1; i < addedProduct.Count; i++)
                Assert.AreSame(testCollection[i], addedProduct[i]);
        }


        [Test]
        public void AllAddTest()
        {
            List<IProduct> addedProduct = new List<IProduct>(GetItemList<Drama>());
            testCollection.AddAll(addedProduct);
            Assert.AreEqual(addedProduct.Count, testCollection.Count);
            for (int i = 1; i < addedProduct.Count; i++)
                Assert.AreSame(testCollection[i], addedProduct[i]);
        }

        [Test]
        public void IndexCheckTetst()
        {
            string namePrefix = "t";
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(10, namePrefix));
            testCollection.AddAll(itemList);
            for (int i = 0; i < itemList.Count; i++)
                Assert.AreSame(testCollection[namePrefix + i], itemList[i]);
        }

        private static void deleteTestHelper(List<IProduct> itemList, Collection testCollection,
            Action<int, List<IProduct>, Collection> deleteAction)
        {
            testCollection.AddAll(itemList);
            Random random = new Random();
            int r;
            for (int i = 0; i < itemList.Count / 5; i++)
            {
                r = random.Next(0, itemList.Count - 1);
                deleteAction(r, itemList, testCollection);
            }
            for (int i = 0; i < itemList.Count; i++)
                Assert.AreSame(testCollection[i], itemList[i]);
        }

        [Test]
        public void DeleteByIndexTest()
        {
            
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100));
            deleteTestHelper(itemList, testCollection, (position, originalList, testCollect) =>
            {
                originalList.RemoveAt(position);
                testCollect.RemoveAt(position);
            });
            
            Assert.Throws<IndexOutOfRangeException>(() => testCollection.RemoveAt(100));
        }

        [Test]
        public void DeleteByNameTest()
        {
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100,namePrefix:"del"));
            deleteTestHelper(itemList, testCollection, (position, originalList, testCollect) =>
            {
                testCollect.RemoveByName(originalList[position].Name);
                originalList.RemoveAt(position);
            });
            
            Assert.Throws<KeyNotFoundException>(() => testCollection.RemoveByName("unknowName"));
        }

        [Test]
        public void DeleteByItemTest()
        {
            List<IProduct> itemList = new List<IProduct>(GetItemList<Drama>(100,namePrefix:"del"));
            deleteTestHelper(itemList, testCollection, (position, originalList, testCollect) =>
            {
                testCollect.Remove(originalList[position]);
                originalList.RemoveAt(position);
            });
            
            Assert.Throws<KeyNotFoundException>(() => testCollection.RemoveByName("unknowName"));
        }

        [Test]
        [Ignore("uses once for perfomance check")]
        public void CompareDeletePerfomance()
        {
            int firstResult = DeleteByNameBacnhmart(new Collection(), (collection, nameForDelete) =>
            {
                collection.RemoveFirst(nameForDelete, (name, product) => product.Name.Equals(name));
            });
            
            
            int secondResult = DeleteByNameBacnhmart(new Collection(), (collection, nameForDelete) =>
            {
                collection.RemoveFirst(nameForDelete, Collection.nameChecker);
            });
            
            int thredResult = DeleteByNameBacnhmart(new Collection(), (collection, nameForDelete) =>
            {
                collection.RemoveByName(nameForDelete);
            });
            
            Console.Write("using lambda :: ");
            Console.Write(firstResult);
            Console.Write("\t");
            Console.Write("using method reference :: ");
            Console.Write(secondResult);
            Console.Write("\t");
            Console.Write("withot lambda:: ");
            Console.Write(thredResult);
            Assert.True(thredResult<secondResult && thredResult<firstResult);
        }

        public static int DeleteByNameBacnhmart(Collection testCollection,Action<Collection,string> deleteAction)
        {
            string namePrefix = "";
            int collectionSize = 20000;
            int section = 50;
            int size = collectionSize/2/section;
            testCollection.AddAll(new List<IProduct>(GetItemList<Drama>(collectionSize, namePrefix)));

            int allTime = 0;
            int dellName = collectionSize / 2 - size;
            for (int i = 0; i < section; i++)
            {
                int start = (testCollection.Count - size) / 2;
                int end = start + size;
                
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                for (int j = start; j < end; j++)
                {
                    deleteAction(testCollection, dellName.ToString());
                    dellName++;
                }
                timer.Stop();
                allTime+= timer.Elapsed.Milliseconds;
            }
            return allTime / section;
        }

        [Test]
        public void SortTest()
        {
            string[] names = {"mock","cort","bor","artica"};
            foreach (string name in names)
            {
                testCollection.Add(new Drama(name:name));
            }
            testCollection.Sort();
            for (int i = 0; i < names.Length; i++)
            {
                Assert.AreEqual(testCollection[i].Name,names[names.Length-1-i]);
            }
        }
    }
}