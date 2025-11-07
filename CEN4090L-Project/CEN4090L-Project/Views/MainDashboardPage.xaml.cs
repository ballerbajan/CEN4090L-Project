using CEN4090L_Project.ViewModels;

namespace CEN4090L_Project.Views;

public partial class MainDashboardPage : ContentPage
{
    public MainDashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardViewModel();
    }
}
