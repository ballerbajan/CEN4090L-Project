using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using CollegeCompanion.Library.Services;

namespace CEN4090L_Project.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private DatabaseService _db => DatabaseService.Current;

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

        private async void Register()
        {
            HasError = false;

            // Validation
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email))
            {
                ShowError("All fields are required.");
                return;
            }

            if (Password.Length < 6)
            {
                ShowError("Password must be at least 6 characters long.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                ShowError("Passwords do not match.");
                return;
            }

            // Store credentials before clearing form
            var tempUsername = Username;
            var tempPassword = Password;
            var tempEmail = Email;
            var tempName = Name;

            Console.WriteLine($"[REGISTER VM] Attempting to register: {tempUsername}");

            // Register with database
            if (_db.Register(tempUsername, tempPassword, tempEmail, tempName))
            {
                Console.WriteLine($"[REGISTER VM] Registration successful for: {tempUsername}");

                // Clear the form
                Username = string.Empty;
                Password = string.Empty;
                Email = string.Empty;
                Name = string.Empty;
                ConfirmPassword = string.Empty;

                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful! Please log in.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                Console.WriteLine($"[REGISTER VM] Registration failed for: {tempUsername}");
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