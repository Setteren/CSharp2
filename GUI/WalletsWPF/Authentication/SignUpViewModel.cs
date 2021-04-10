
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Wallets.BusinessLayer;
using Wallets.GUI.WPF.Annotations;
using Wallets.GUI.WPF.Navigation;
using Wallets.Models.Users;
using Wallets.Services;

namespace Wallets.GUI.WPF.Authentication
{
    public class SignUpViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private RegistrationUser _regUser = new RegistrationUser();
        private Action _gotoSignIn;
        private bool _isEnabled = true;


        public AuthNavigatableTypes Type
        {
            get
            {
                return AuthNavigatableTypes.SignUp;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }


        public string Login
        {
            get
            {
                return _regUser.Login;
            }
            set
            {
                if (_regUser.Login != value)
                {
                    _regUser.Login = value;
                    OnPropertyChanged(nameof(Login));
                    SignUpCommand.RaiseCanExecuteChanged();
                }
               
            }
        }

        public string Password
        {
            get
            {
                return _regUser.Password;
            }
            set
            {
                if (_regUser.Password != value)
                {
                    _regUser.Password = value;
                    OnPropertyChanged(nameof(Password));
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public string LastName
        {
            get
            {
                return _regUser.LastName;
            }
            set
            {
                if (_regUser.LastName != value)
                {
                    _regUser.LastName = value;
                    OnPropertyChanged(nameof(LastName));
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string FirstName
        {
            get
            {
                return _regUser.FirstName;
            }
            set
            {
                if (_regUser.FirstName != value)
                {
                    _regUser.FirstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get
            {
                return _regUser.Email;
            }
            set
            {
                if (_regUser.Email != value)
                {
                    _regUser.Email = value;
                    OnPropertyChanged(nameof(Email));
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand SignInCommand { get; }

        public SignUpViewModel(Action gotoSignIn)
        {
            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            CloseCommand = new DelegateCommand(()=>Environment.Exit(0));
            _gotoSignIn = gotoSignIn;
            SignInCommand = new DelegateCommand(_gotoSignIn);
        }

        private bool IsSignUpEnabled()
        {
            
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(LastName) && !String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(Email);
        }

        private async void SignUp()
        {
            var authService = new AuthenticationService();
                Client user = null;
                try
                {
                    IsEnabled = false; 
                    await Task.Run(()=>authService.RegistrateUser(_regUser));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sign up failed {ex.Message}");
                    return;
                }
                finally // is done independently from exception
                {
                    IsEnabled = true;
                }
                 MessageBox.Show("User successfully registered, please Sign In.");
                _gotoSignIn.Invoke();

            
        }



        public void ClearSensitiveData()
        {
            _regUser = new RegistrationUser();
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
