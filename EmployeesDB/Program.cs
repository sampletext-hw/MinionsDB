using System;
using System.Linq;
using EmployeesDB.Data.Models;

namespace EmployeesDB
{
    class Program
    {
        private static EmployeesContext _context = new EmployeesContext();
        static void Main(string[] args)
        {
        }

        private static string GetEmployeesInformation()
        {
            var employees = _context.Employees.OrderBy(e => e.EmployeeId).Select(e => e).ToList();
            return string.Join("\n",
                employees.Select(t =>
                    $"{t.EmployeeId} - {t.FirstName} - {t.LastName} - {t.MiddleName} - {t.JobTitle}"));
        }

        private static void TestTask3()
        {
            Console.WriteLine(GetEmployeesInformation());
        }
    }
}
