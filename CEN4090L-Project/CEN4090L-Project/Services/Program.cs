using System;
using CEN4090L_Project.Services;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.TestCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing Group & Transaction Service Proxies ===\n");

            var groupProxy = GroupServiceProxy.Current;
            var transactionProxy = TransactionServiceProxy.Current;

            // Check that group proxy works
            Console.WriteLine("Current Users:");
            foreach (var u in groupProxy.Users)
            {
                Console.WriteLine($"- {u.Username} ({u.Email}) | Budget: {u.Budget?.Title ?? "None"}");
            }

            Console.WriteLine("\nAdding new budget via TransactionServiceProxy...");
            bool success = transactionProxy.AddOrUpdateBudget("Weekend Spending", 2, 400f);

            if (success)
                Console.WriteLine("Budget added successfully.\n");

            Console.WriteLine("Updated user info:");
            var currentUser = groupProxy.Users[0];
            Console.WriteLine($"User: {currentUser.Username}");
            Console.WriteLine($"Budget Title: {currentUser.Budget.Title}");
            Console.WriteLine($"Needs: {currentUser.Budget.Needs}");
            Console.WriteLine($"Wants: {currentUser.Budget.Wants}");
            Console.WriteLine($"Savings: {currentUser.Budget.Savings}");

            Console.WriteLine("\n=== Test Complete ===");
            Console.ReadKey();
        }
    }
}
