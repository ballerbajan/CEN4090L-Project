using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEN4090L_Project.Services;
using CEN4090L_Project.Models;

namespace CEN4090L_Project.Services
{
    public class TransactionServiceProxy
    {
        // should be get a singleton inst of group prox and run on that
        private GroupServiceProxy groupService = GroupServiceProxy.Current;
        // should be this
        // var user = groupService.currentUser;
        // for now just get the first user
        User user = groupService.Users[0];

        // Singleton instance so we have a single proxy throughout the app
        private static TransactionServiceProxy? instance = null;
        public static TransactionServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransactionServiceProxy();
                }
                return instance;
            }
        }
        // Private constructor
        private TransactionServiceProxy()
        {
            // testing data
            user.Budget.Add(new Budget() { Title = "Food", Priority = 1 });
        }

        // this should take in a budget object but for now just the fields
        public bool AddOrUpdateBudget(string title, int category, float amount)
        {
            // for now lets do the budget splitting here

            float needs = amount * .5f; // 50% needs
            float wants = amount * .3f;
            float savings = amount * .2f;

            // this will be replaced by the budget object passed in
            // this is creating a new budget each time
            // new code can be added to update existing budget when we have the
            // full budget object
            var newBudget = new Budget()
            {
                Title = title,
                Category = category,
                Needs = needs,
                Wants = wants,
                Savings = savings
            };

            user.Budget = newBudget;
        }
    }
}
