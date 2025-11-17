using CEN4090L_Project.ViewModels;

namespace CEN4090L_Project.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}