using System;

namespace Task1
{
    public class Program
    {
        static void Main(string[] args)
        {
            MyStack<Minion> minions = new MyStack<Minion>();

            minions.Push(new Minion(1, "Bob", 17, 1));
            minions.Push(new Minion(2, "Alex", 23, 3));
            minions.Push(new Minion(3, "Charlie", 41, 2));

            foreach (var minion in minions)
            {
                Console.WriteLine(minion);
            }
        }
    }
}