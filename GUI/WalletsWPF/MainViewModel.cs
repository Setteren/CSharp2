using Wallets.GUI.WPF.Authentication;
using Wallets.GUI.WPF.Navigation;
using Wallets.GUI.WPF.Wallets;

namespace Wallets.GUI.WPF
{

    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {


        public MainViewModel()
        {
            Navigate(MainNavigatableTypes.Auth);
        }



        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            if (type == MainNavigatableTypes.Auth)
            {
                return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets));
            }
            else
            {
                if (type == MainNavigatableTypes.Wallets)
                {
                    return new WalletsViewModel(() => Navigate(MainNavigatableTypes.Auth),
                        () => Navigate(MainNavigatableTypes.AddWallet));
                }

                else //if (type == MainNavigatableTypes.AddWallet)
                {
                    return new AddWalletViewModel(() => Navigate(MainNavigatableTypes.Wallets));
                }
            }

        }


    }
}
