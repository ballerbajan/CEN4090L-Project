using CEN4090L_Project.ViewModels;
namespace CEN4090L_Project.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
		BindingContext = new RegisterViewModel();
    }
}