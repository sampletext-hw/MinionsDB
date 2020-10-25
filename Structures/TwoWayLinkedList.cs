using System;
using System.Collections.Generic;

namespace Task1
{
    public class TwoWayLinkedList<T>
    {
        public int Count { get; private set; }
        public Node<T> Head { get; private set; }
        public Node<T> Last { get; private set; }

        public T this[int index]
        {
            get => Get(index);
            set => Set(value, index);
        }

        public void Add(T value)
        {
            Node<T> node = new Node<T>(value);
            if (Count == 0)
            {
                Head = node;
                Last = node;
            }
            else
            {
                Last.Next = node;
                node.Previous = Last;
                Last = node;
            }

            Count++;
        }

        private Node<T> GetNode(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"Index {index} is invalid; Only {Count} elements exist");

            Node<T> current = Head;
            int k = 0;
            while (k < index)
            {
                current = current.Next;
                k++;
            }

            return current;
        }

        public T Get(int index)
        {
            return GetNode(index).Value;
        }

        public void InsertBefore(T value, int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"{index} is invalid; Only {Count} elements exist");

            if (index == Count - 1)
            {
                Add(value);
                return;
            }

            Node<T> node = new Node<T>(value);

            if (index == 0)
            {
                node.Next = Head;
                Head.Previous = node;
                Head = node;
                Count++;
                return;
            }

            // move to element before insert
            Node<T> current = GetNode(index);

            // link back
            current.Previous.Next = node;
            node.Previous = current.Previous;

            //link forward
            node.Next = current;
            current.Previous = node;

            Count++;
        }

        public void RemoveAt(int index)
        {
            var node = GetNode(index);

            if (Head == node)
            {
                if (Head.Next != null)
                {
                    Head.Next.Previous = null;
                    Head = Head.Next;
                }
                else
                {
                    Head = null;
                }
            }
            else if (Last == node)
            {
                if (Last.Previous != null)
                {
                    Last.Previous.Next = null;
                    Last = Last.Previous;
                }
                else
                {
                    Last = null;
                }
            }
            else
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }

            Count--;
        }

        public void Set(T value, int index)
        {
            GetNode(index).Value = value;
        }

        public override string ToString()
        {
            return $"{{ {nameof(Count)}: {Count}, Items:\n{string.Join("\n", GetEnumerator())}}}";
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = Head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        public TwoWayLinkedList()
        {
        }
    }
}