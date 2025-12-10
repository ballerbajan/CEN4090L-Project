using System.ComponentModel;
using System.Runtime.CompilerServices;
using CEN4090L_Project.Views;
using CEN4090L_Project.Models;
using CollegeCompanion.Library.Services;

namespace CEN4090L_Project.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // CHANGED: Use DatabaseService instead of GroupServiceProxy
        private DatabaseService _db => DatabaseService.Current;

        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
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
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password.", "OK");
                Password = string.Empty;
                return;
            }

            // Store credentials before clearing
            var loginUsername = Username.Trim();
            var loginPassword = Password;

            Console.WriteLine($"[LOGIN VM] Attempting login with: {loginUsername}");

            // CHANGED: Use DatabaseService instead of GroupServiceProxy
            if (_db.Login(loginUsername, loginPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");
                Password = string.Empty;
                await Application.Current.MainPage.Navigation.PushAsync(new Views.MainDashboardPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
                Password = string.Empty;
            }
        }

        private async void OnRegister()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.RegisterPage());
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