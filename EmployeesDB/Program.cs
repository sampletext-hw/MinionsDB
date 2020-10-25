using System;
using System.Linq;
using EmployeesDB.Data.Models;

namespace EmployeesDB
{
    class Program
    {
        public static EmployeesContext Context { get; } = new EmployeesContext();

        static void Main(string[] args)
        {
            Task1();
        }

        private static string GetEmployeesInformation()
        {
            var employees = Context.Employees.OrderBy(e => e.EmployeeId).Select(e => e).ToList();
            return string.Join("\n",
                employees.Select(t =>
                    $"{t.EmployeeId} - {t.FirstName} - {t.LastName} - {t.MiddleName} - {t.JobTitle}"));
        }

        private static void TestTask3()
        {
            Console.WriteLine(GetEmployeesInformation());
        }

        private static void Task1()
        {
            Console.WriteLine(string.Join("\n", Context.Employees.Where(t => t.Salary > 48000).OrderBy(t => t.LastName).ToList()));
        }

        private static void Task2()
        {
            var town = new Towns(){Name = "Moscow"};
            Context.Towns.Add(town);
            Context.SaveChanges(); //call to get town ID

            var address = new Addresses() {AddressText = "27/1 Lubyanka", Town = town};
            Context.Addresses.Add(address);
            Context.SaveChanges(); //call to get address ID

            var browns = Context.Employees.Where(t => t.LastName == "Brown").ToList();
            browns.ForEach(t=>t.Address = address);
            Context.SaveChanges(); //call to save addresses
        }
    }
}
