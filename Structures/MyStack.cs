using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public class MyStack<T> where T : class
    {
        public T[] InnerArray { get; private set; }
        public int Capacity { get; private set; }
        public int Amount { get; private set; }

        public MyStack(int capacity)
        {
            InnerArray = new T[capacity];
            Capacity = capacity;
            Amount = 0;
        }

        public MyStack() : this(4)
        {
        }

        private void Resize(int capacity)
        {
            T[] array = new T[capacity];
            Array.Copy(InnerArray, array, Capacity);
            Capacity = capacity;
            InnerArray = array;
        }

        public void Push(T element)
        {
            if (Amount == Capacity)
            {
                Resize(Capacity * 2);
            }

            InnerArray[Amount++] = element;
        }

        public T Pop()
        {
            if (Amount == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            Amount--;
            T element = InnerArray[Amount];
            InnerArray[Amount] = null;
            return element;
        }

        public IEnumerator<T> GetEnumerator()
        {
            int amount = Amount;
            for (int i = 0; i < amount; i++)
            {
                yield return Pop();
            }
        }
    }
}