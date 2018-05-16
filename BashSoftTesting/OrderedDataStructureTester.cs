namespace BashSoftTesting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using NUnit.Framework;

    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [Test]
        public void TestEmptyCtor()
        {
            int defaultCapacity = 16;
            int defaultSize = 0;

            this.names = new SimpleSortedList<string>();

            Assert.That(this.names.Capacity, Is.EqualTo(defaultCapacity));
            Assert.That(this.names.Size, Is.EqualTo(defaultSize));
        }
        [Test]
        public void TestCtorWithInitialCapacity()
        {
            int initialCapacity = 50;
            int defaultSize = 0;

            this.names = new SimpleSortedList<string>(initialCapacity);

            Assert.That(this.names.Capacity, Is.EqualTo(initialCapacity));
            Assert.That(this.names.Size, Is.EqualTo(defaultSize));
        }

        [Test]
        public void TestCtorWithInitialComparer()
        {
            int defaultCapacity = 16;
            int defaultSize = 0;
            IComparer<string> comparer = Comparer<string>.Default;

            this.names = new SimpleSortedList<string>(comparer);

            Assert.That(this.names.Capacity, Is.EqualTo(defaultCapacity));
            Assert.That(this.names.Size, Is.EqualTo(defaultSize));
        }

        [Test]
        public void TestCtorWithAllParameters()
        {
            int initialCapacity = 50;
            int defaultSize = 0;
            IComparer<string> comparer = Comparer<string>.Default;

            this.names = new SimpleSortedList<string>(comparer, initialCapacity);

            Assert.That(this.names.Capacity, Is.EqualTo(initialCapacity));
            Assert.That(this.names.Size, Is.EqualTo(defaultSize));
        }

        [SetUp]
        public void SetUp()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestAddIncreasesSize()
        {
            string nameToAdd = "Pesho";

            this.names.Add(nameToAdd);

            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestAddNullThrowsException()
        {
            string input = null;

            Assert.Throws<ArgumentNullException>(() => this.names.Add(input));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            string[] unsortedNames = new string[] { "Rosen", "Georgi", "Balkan" };
            string[] sortedNames = unsortedNames.OrderBy(n => n).ToArray();

            for (int i = 0; i < unsortedNames.Length; i++)
            {
                this.names.Add(unsortedNames[i]);
            }

            int index = 0;

            foreach (var name in this.names)
            {
                Assert.That(name, Is.EqualTo(sortedNames[index]));
                index++;
            }

        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            string[] namesToAdd = new string[] 
            {
                "Rosen", "Georgi", "Balkan",
                "Rosen", "Georgi", "Balkan",
                "Rosen", "Georgi", "Balkan",
                "Rosen", "Georgi", "Balkan",
                "Rosen", "Georgi", "Balkan",
                "Rosen", "Georgi"
            };
            int initialCapacity = 16;

            for (int i = 0; i < namesToAdd.Length; i++)
            {
                this.names.Add(namesToAdd[i]);
            }

            Assert.That(names.Capacity, Is.Not.EqualTo(initialCapacity));
            Assert.That(names.Size, Is.EqualTo(namesToAdd.Length));
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };

            this.names.AddAll(namesToAdd);

            Assert.That(names.Size, Is.EqualTo(namesToAdd.Length));
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            string[] namesToAdd = new string[] { null };

            Assert.Throws<ArgumentNullException>(() => this.names.AddAll(namesToAdd));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            string[] unsortedNames = new string[] { "Rosen", "Georgi", "Balkan" };
            string[] sortedNames = unsortedNames.OrderBy(n => n).ToArray();

            this.names.AddAll(unsortedNames);

            int index = 0;

            foreach (var name in this.names)
            {
                Assert.That(name, Is.EqualTo(sortedNames[index]));
                index++;
            }
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };

            this.names.AddAll(namesToAdd);
            int initialSize = this.names.Size;
            this.names.Remove("Georgi");
            int afterRemoveSize = this.names.Size;

            Assert.That(this.names.Size, Is.Not.EqualTo(initialSize));
            Assert.That(this.names.Size, Is.EqualTo(afterRemoveSize));
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };

            this.names.AddAll(namesToAdd);
            string nameToRemove = "Georgi";
            this.names.Remove(nameToRemove);

            foreach (var name in this.names)
            {
                Assert.That(name, Is.Not.EqualTo(nameToRemove));
            }
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };

            this.names.AddAll(namesToAdd);
            string nameToRemove = null;

            Assert.Throws<ArgumentNullException>(() => this.names.Remove(nameToRemove));
        }

        [Test]
        public void TestJoinWithNull()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };

            this.names.AddAll(namesToAdd);
            string joiner = null;

            Assert.Throws<ArgumentNullException>(() => this.names.JoinWith(joiner));
        }

        [Test]
        public void TestJoinWorksFine()
        {
            string[] namesToAdd = new string[] { "Rosen", "Georgi", "Balkan" };
            string[] sortedNames = namesToAdd.OrderBy(n => n).ToArray();


            this.names.AddAll(namesToAdd);
            string joiner = ", ";

            Assert.That(this.names.JoinWith(joiner), Is.EqualTo(String.Join(joiner, sortedNames)));
        }
    }
}
