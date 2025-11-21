using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CEN4090L_Project.Services;

namespace CEN4090L_Project.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private GroupServiceProxy _service => GroupServiceProxy.Current;

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? ConfirmPassword { get; set; }

        public string? ErrorMessage { get; set; }
        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(Register);
            GoToLoginCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//LoginPage");
            });
        }

        private void Register()
        {
            HasError = false;

            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email)
                )

            {
                ShowError("All fields are required.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                ShowError("Passwords do not match.");
                return;
            }

            if (_service.Register(Username, Password, Name, Email))
            {
                Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ShowError("Username already exists.");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

