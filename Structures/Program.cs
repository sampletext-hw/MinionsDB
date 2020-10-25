using System;

namespace Task1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var minions = new TwoWayLinkedList<Minion>();

            minions.Add(new Minion(1, "Bob", 17, 1));
            minions.Add(new Minion(2, "Alex", 23, 3));
            minions.Add(new Minion(3, "Charlie", 41, 2));

            foreach (var minion in minions)
            {
                Console.WriteLine(minion);
            }
        }
    }
}