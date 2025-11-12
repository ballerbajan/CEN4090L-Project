using CEN4090L_Project.ViewModels;

namespace CEN4090L_Project.Views;

public partial class MainDashboardPage : ContentPage
{
    public MainDashboardPage()
    {
        InitializeComponent();
        //BindingContext = new DashboardViewModel();
        BindingContext = DashboardViewModel.Instance;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Force refresh when returning to page
        if (BindingContext is DashboardViewModel vm)
        {
            vm.OnPropertyChanged(nameof(vm.RecentExpenses));
        }
    }

}
