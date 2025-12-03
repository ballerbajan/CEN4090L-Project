namespace WebApi.Enterprise
{
    public class ExpenseEC
    {
        public IEnumerable<Expense> GetExpenses()
        {
            return FakeDatabase.CurrentUser.Expenses;
        }

        public Expense? GetExpenseById(int id)
        {
            return FakeDatabase.CurrentUser.Expenses
                .FirstOrDefault(t => t.Id == id);
        }

        public Expense? Delete(int id)
        {
            var expenseToDelete = GetExpenseById(id);
            if (expenseToDelete != null)
            {
                FakeDatabase.CurrentUser.Expenses.Remove(expenseToDelete);
            }
            return expenseToDelete;
        }

        public Expense? AddOrUpdate(Expense? expense)
        {
            if (expense != null && expense.Id == 0)
            {
                expense.Id = FakeDatabase.NextExKey;
                FakeDatabase.CurrentUser.Expenses.Add(expense);
            }

            return expense;
        }
    }
}
