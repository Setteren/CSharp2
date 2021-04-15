

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Mvvm;
using Wallets.BusinessLayer;
using Wallets.GUI.WPF.Navigation;
using Wallets.Services;

namespace Wallets.GUI.WPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        public static WalletService _service=new WalletService();
        public static TransactionService _tranService = new TransactionService();
        private  WalletDetailsViewModel _currentWallet;
        private Action _gotoSignIn;
        private Action _gotoAddWallet;
        private Action _gotoAddCategory;
        private Action _gotoDeleteCategory;
        private bool _isEnabled = true;


        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }


        public static ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get { return _currentWallet;}
            set
            { 
                _currentWallet = value;
                if (_currentWallet != null)
                {
                    _currentWallet.UpdateListsOfCategories();
                    _currentWallet.UpdateTransactionsCollection();
                    _currentWallet.BeginningOfTransactionList = 0;
                    _currentWallet.ShowTenTransactions();
                }
                
                RaisePropertyChanged();
            }
        }

        public WalletsViewModel(Action gotoSignIn, Action goToAddWallet, Action goToAddCategory, Action goToDeleteCategory)
        {
            Wallets = new ObservableCollection<WalletDetailsViewModel>();

            _gotoSignIn = gotoSignIn;
            _gotoAddWallet = goToAddWallet;
            _gotoAddCategory = goToAddCategory;
            _gotoDeleteCategory = goToDeleteCategory;

            AddWalletCommand = new DelegateCommand(_gotoAddWallet);
            SignInCommand = new DelegateCommand(_gotoSignIn);
            AddUserCategoryCommand = new DelegateCommand(_gotoAddCategory); 
            DeleteUserCategoryCommand = new DelegateCommand(_gotoDeleteCategory);

            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
        }

        public static void UpdateWalletsCollection()
        {
      
            Wallets.Clear();
            foreach (Wallet wallet in _service.GetWallets())
            {
                if (wallet.OwnerGuid == MainInfo.User.Guid)
                {
                    Wallets.Add(new WalletDetailsViewModel(wallet));
                }
            }
        }

        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand SignInCommand { get; }
        public DelegateCommand AddUserCategoryCommand { get; }
        public DelegateCommand DeleteUserCategoryCommand { get; }
        public DelegateCommand DeleteWalletCommand { get; }
        public DelegateCommand CloseCommand { get; }


        

        public async  void DeleteWallet()
        {
            try
            {
                IsEnabled = false;

                foreach (Transaction transaction in _tranService.GetTransactions())
                {
                    if (transaction.WalletGuid == _currentWallet.FromWallet.Guid)
                    {
                        if (_service.GetWallet(_currentWallet.FromWallet).Transactions.ToList().Exists(x => x.Guid == transaction.Guid))
                        {
                            _tranService.DeleteTransaction(transaction);
                        }
                    }
                }

                await Task.Run(()=>_service.DeleteWallet(CurrentWallet.FromWallet));
                Wallets.Remove(CurrentWallet);



            }
            catch (Exception ex)
            {
                MessageBox.Show("Please,select the wallet by clicking on it in the list");
                return;
            }
            finally
            {
                IsEnabled = true;
            }
        }

        public MainNavigatableTypes Type {
            get
            {
                return MainNavigatableTypes.Wallets;
            }
        }
        public void ClearSensitiveData()
        {
            CurrentWallet = null;
            UpdateWalletsCollection();
        }


     

    }
}
