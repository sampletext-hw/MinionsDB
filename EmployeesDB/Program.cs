﻿using System;
using System.Linq;
using EmployeesDB.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesDB
{
    class Program
    {
        public static EmployeesContext Context { get; } = new EmployeesContext();

        static void Main(string[] args)
        {
            Task3();
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
            Console.WriteLine(string.Join("\n",
                Context.Employees.Where(t => t.Salary > 48000).OrderBy(t => t.LastName).ToList()));
        }

        private static void Task2()
        {
            var town = new Towns() {Name = "Moscow"};
            Context.Towns.Add(town);
            Context.SaveChanges(); //call to get town ID

            var address = new Addresses() {AddressText = "27/1 Lubyanka", Town = town};
            Context.Addresses.Add(address);
            Context.SaveChanges(); //call to get address ID

            var browns = Context.Employees.Where(t => t.LastName == "Brown").ToList();
            browns.ForEach(t => t.Address = address);
            Context.SaveChanges(); //call to save addresses
        }

        private static void Task3()
        {
            var employees = Context.Employees
                .Where(e => e.EmployeesProjects
                    .Select(ep => ep.ProjectId)
                    .Any(pid =>
                        Context.Projects
                            .Where(p => 2002 <= p.StartDate.Year && p.StartDate.Year <= 2005)
                            .Select(p => p.ProjectId).Contains(pid)
                    )
                )
                .Include(e => e.EmployeesProjects)
                .ThenInclude(ep => ep.Project)
                .Include(e => e.Manager)
                .Take(5)
                .ToList();

            // как это работает?
            // 
            //  выбираем таких сотрудников, у которых
            //   в связях проектов
            //       из которых выбрали Id проектов
            //           хотя бы 1 такой проект,
            //           который содержится в выборке из проектов по году
            //           из которых выбрали Id проекта
            //  включая связи проектов
            //  для которых включаем сам проект
            //  включая менеджера сотрудника
            //  первые 5

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.Manager.FirstName} {employee.Manager.LastName}");
                var projects = employee.EmployeesProjects.Select(t => t.Project);
                foreach (var project in projects)
                {
                    if (2002 <= project.StartDate.Year && project.StartDate.Year <= 2005)
                    {
                        Console.WriteLine("\t{0} - {1:dd.MM.yyyy} - {2}", project.Name, project.StartDate,
                            (project.EndDate.HasValue ? project.EndDate.Value.ToString("dd.MM.yyyy") : "Не завершён"));
                    }
                }
            }
        }
    }
}