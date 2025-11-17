using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CEN4090L_Project.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        // Commands
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ForgotPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
            RegisterCommand = new Command(OnRegister);
            ForgotPasswordCommand = new Command(OnForgotPassword);
        }

        private async void OnLogin()
        {
            // TODO: Add authentication logic here
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // For now, show success message and navigate to dashboard
            await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new Views.MainDashboardPage());
        }

        private async void OnRegister()
        {
            await Application.Current.MainPage.DisplayAlert("Info", "Navigate to Register Page (to be implemented).", "OK");
        }

        private async void OnForgotPassword()
        {
            await Application.Current.MainPage.DisplayAlert("Info", "Forgot Password feature coming soon.", "OK");
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}