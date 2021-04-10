using System;
using System.Windows;
using System.Windows.Controls;

namespace Wallets.GUI.WPF.Authentication
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignInView : UserControl
    {

        public SignInView()
        {
            InitializeComponent();
        }


        // Because PasswordBox do not have Binding, we do that in a code-behind file
        private void TbPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((SignInViewModel)DataContext).Password = TbPassword.Password;
        }
    }
}
