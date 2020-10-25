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
    }
}
