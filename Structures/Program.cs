using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structures
{
    public class Program
    {
        static void Main(string[] args)
        {
            TwoWayLinkedList<Minion> minions = new TwoWayLinkedList<Minion>();

            minions.Add(new Minion(1, "Bob", 17, 1));
            minions.Add(new Minion(2, "Alex", 23, 3));
            minions.Add(new Minion(3, "Charlie", 41, 2));

            minions.InsertBefore(new Minion(4, "Jessy", 27, 4), 1);
            minions.RemoveAt(5);

            Console.WriteLine(minions);
        }
    }

    public class Minion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int TownId { get; set; }

        public Minion(int id, string name, int age, int townId)
        {
            Id = id;
            Name = name;
            Age = age;
            TownId = townId;
        }

        public override string ToString()
        {
            return
                $"{{ {nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(TownId)}: {TownId} }}";
        }
    }

    public class Node<T>
    {
        public Node<T> Previous { get; set; }
        public Node<T> Next { get; set; }
        public T Value { get; set; }

        public Node(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }

    public class TwoWayLinkedList<T>
    {
        public int Count { get; private set; }
        public Node<T> Head { get; private set; }
        public Node<T> Last { get; private set; }

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
            return $"{{ {nameof(Count)}: {Count}, Items:\n{string.Join("\n", AsEnumerable())}}}";
        }

        public IEnumerable<T> AsEnumerable()
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