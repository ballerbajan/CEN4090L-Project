using CEN4090L_Project.Models;

namespace WebApi.Enterprise
{
    public class SavingGoalEC
    {
        public IEnumerable<SavingsGoal> GetSavingsGoals()
        {
            return FakeDatabase.CurrentUser.SavingsGoals;
        }

        public SavingsGoal? GetSavingsGoalById(int id)
        {
            return FakeDatabase.CurrentUser.SavingsGoals
                .FirstOrDefault(t => t.Id == id);
        }

        public SavingsGoal? Delete(int id)
        {
            var savingsGoalToDelete = GetSavingsGoalById(id);
            if (savingsGoalToDelete != null)
            {
                FakeDatabase.CurrentUser.Expenses.Remove(savingsGoalToDelete);
            }
            return savingsGoalToDelete;
        }

        public SavingsGoal? AddOrUpdate(SavingsGoal? savingsGoal)
        {
            if (savingsGoal != null && savingsGoal.Id == 0)
            {
                savingsGoal.Id = FakeDatabase.NextSGKey;
                FakeDatabase.CurrentUser.SavingsGoals.Add(savingsGoal);
            }

            return savingsGoal;
        }
    }
}
