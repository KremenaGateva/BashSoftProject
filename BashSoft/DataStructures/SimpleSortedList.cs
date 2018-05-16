namespace BashSoft.DataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using BashSoft.Contracts;

    public class SimpleSortedList<T> : ISimpleOrderedBag<T>
        where T : IComparable<T>
    {
        private const int DefaultSize = 16;

        private T[] innerCollection;
        private int size;
        private IComparer<T> comparison;

        private void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Capacity cannot be negative!");
            }
            this.innerCollection = new T[capacity];
        }

        public SimpleSortedList(IComparer<T> comparer, int capacity)
        {
            this.InitializeInnerCollection(capacity);
            this.comparison = comparer;
        }

        public SimpleSortedList(int capacity)
            : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), capacity)
        {
        }

        public SimpleSortedList(IComparer<T> comparer)
            :this(comparer, DefaultSize)
        {
        }

        public SimpleSortedList()
            :this(Comparer<T>.Create((x, y) => x.CompareTo(y)), DefaultSize)
        {
        }

        public int Size
        {
            get => this.size;
        }

        public int Capacity
        {
            get
            {
                return this.innerCollection.Length;
            }
        }

        public void Add(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element", "Cannot be null.");
            }
            if (this.innerCollection.Length == this.Size)
            {
                Resize();
            }
            this.innerCollection[size] = element;
            size++;

            Array.Sort(this.innerCollection, 0, this.Size, this.comparison);
        }

        private void Resize()
        {
            T[] newCollection = new T[this.Size * 2];
            Array.Copy(this.innerCollection, newCollection, this.Size);
            this.innerCollection = newCollection;
        }

        public void AddAll(ICollection<T> collection)
        {
            if (this.Size + collection.Count >= this.innerCollection.Length)
            {
                MultiResize(collection);
            }

            foreach (var element in collection)
            {
                if (element == null)
                {
                    throw new ArgumentNullException("element", "Cannot be null.");
                }
                this.innerCollection[this.size] = element;
                this.size++;
            }
            Array.Sort(this.innerCollection, 0, this.Size, this.comparison);
        }

        private void MultiResize(ICollection<T> collection)
        {
            int newSize = this.innerCollection.Length * 2;
            while (this.Size + collection.Count >= newSize)
            {
                newSize *= 2;
            }

            T[] newCollection = new T[newSize];
            Array.Copy(this.innerCollection, newCollection, this.size);
            this.innerCollection = newCollection;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Size; i++)
            {
                yield return this.innerCollection[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public string JoinWith(string joiner)
        {
            if (joiner == null)
            {
                throw new ArgumentNullException("joiner", "Cannot be null");
            }
            StringBuilder sb = new StringBuilder();

            foreach (var element in this.innerCollection)
            {
                if (element == null)
                {
                    break;
                }
                sb.Append(element);
                sb.Append(joiner);
            }

            sb.Remove(sb.Length - joiner.Length, joiner.Length);
            return sb.ToString();
        }

        public bool Remove(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element", "Cannot be null.");
            }
            bool hasBeenRemoved = false;
            int indexOfRemoved = 0;

            for (int i = 0; i < this.Size; i++)
            {
                if (this.innerCollection[i].Equals(element))
                {
                    indexOfRemoved = i;
                    this.innerCollection[i] = default(T);
                    this.size--;
                    hasBeenRemoved = true;
                    break;
                }
            }

            if (hasBeenRemoved)
            {
                for (int i = indexOfRemoved; i < this.Size - 1; i++)
                {
                    this.innerCollection[i] = this.innerCollection[i + 1];
                }

                this.innerCollection[this.Size - 1] = default(T);
            }

            return hasBeenRemoved;
        }
    }
}
