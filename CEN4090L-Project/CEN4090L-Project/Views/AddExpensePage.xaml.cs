using CEN4090L_Project.ViewModels;

namespace CEN4090L_Project.Views
{
    public partial class AddExpensePage : ContentPage
    {
        public AddExpensePage()
        {
            InitializeComponent();
            BindingContext = new ExpensePageViewModel();
        }
    }
}
