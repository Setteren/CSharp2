using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Prism.Commands;
using Prism.Mvvm;
using Wallets.BusinessLayer;
using Wallets.GUI.WPF.Annotations;
using Wallets.GUI.WPF.Navigation;

using Wallets.Services;

namespace Wallets.GUI.WPF.Wallets
{
    public class WalletDetailsViewModel: BindableBase
    {
        private WalletService _walService;
        private CategoryService _catService;
        private TransactionService _tranService;


        //FOR WALLET
        private Wallet _wallet;
        private decimal _usdAmount;
        private decimal _eurAmount;
        private decimal _uahAmount;
        private decimal _rubAmount;
        private bool _isEnabled = true;

        public Wallet FromWallet { get { return _wallet; } }


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

        public string Name
        {
            get
            {
                return _wallet.Name;
            }
            set
            {
                _wallet.Name = value;
            }
        }


        public decimal Balance
        {
            get
            {
                return _wallet.Balance;
            }
            set
            {
                _wallet.Balance = value;
            }
        }


        public string Description
        {
            get
            {
                return _wallet.Description;
            }
            set
            {
                _wallet.Description = value;
            }
        }

        public Currency? MainCurrency
        {
            get
            {
                return _wallet.MainCurrency;
            }
            set
            {
                if (value == Currency.USD) { _wallet.Balance = _usdAmount; RaisePropertyChanged(nameof(Balance)); }
                if (value == Currency.EUR) { _wallet.Balance = _eurAmount; RaisePropertyChanged(nameof(Balance)); }
                if (value == Currency.UAH) { _wallet.Balance = _uahAmount; RaisePropertyChanged(nameof(Balance)); }
                if (value == Currency.RUB) { _wallet.Balance = _rubAmount; RaisePropertyChanged(nameof(Balance)); }

                _wallet.MainCurrency = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name} ({_wallet.Balance} {_wallet.MainCurrency}) ";
            }
        }




        //GETTING DATA TO WRITE
        private Currency? _startingCurrency;
        public Currency? StartingCurrency
        {
            get { return _startingCurrency; }
            set { _startingCurrency = value; }
        }

        public decimal LastMonthIncome
        {
            get { return _wallet.LastMonthIncome(); }
        }

        public decimal LastMonthExpense
        {
            get { return _wallet.LastMonthExpense(); }
        }




        //FOR CATEGORIES
        private Category _selectedCategory;
        public static ObservableCollection<Category> WalletCategoriesAdded { get; set; }
        public static ObservableCollection<Category> WalletCategoriesAvailable { get; set; }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged();
            }
        }


        //FOR TRANSACTIONS
        private Transaction _currentTransaction;
        private Transaction _uneditedTransaction;
        private Transaction _addTransaction;
        private int _beginningOfTransactionList;
        private ObservableCollection<Transaction> _tenTransactions;

        //FOR EDITED AND DELETED TRANSACTIONS
        public Transaction CurrentTransaction
        {
            get
            {
                //_editTransaction = _currentTransaction;
                return _currentTransaction;
            }
            set
            {
                _currentTransaction = value;
                _uneditedTransaction = new Transaction(_currentTransaction.WalletGuid,_currentTransaction.MoneyAmount,_currentTransaction.Currency,
                    _currentTransaction.Category,_currentTransaction.Description,_currentTransaction.Date,_currentTransaction.Guid);
                
                    RaisePropertyChanged();
            }
        }

        public Transaction UneditedTransaction
        {
            get
            {
                return _uneditedTransaction;
            }
            set
            {
                _uneditedTransaction = value;
            }
        }
    


        // FOR ADDED TRANSACTION

        public decimal AddTransactionMoneyAmount
        {
            get { return _addTransaction.MoneyAmount; }
            set
            {
                _addTransaction.MoneyAmount = value;
                RaisePropertyChanged();
            }
        }

        public Currency? AddTransactionCurrency
        {
            get
            {
                return _addTransaction.Currency;
            }
            set
            {
                _addTransaction.Currency = value;
                RaisePropertyChanged();
            }
        }


        public Category AddTransactionCategory
        {
            get
            {
                return _addTransaction.Category;
            }
            set
            {
                _addTransaction.Category = value;
                RaisePropertyChanged();
            }
        }

        public string AddTransactionDescription
        {
            get
            {
                return _addTransaction.Description;
            }
            set
            {
                _addTransaction.Description = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? AddTransactionDate
        {
            get
            {
                return _addTransaction.Date;
            }
            set
            {
                _addTransaction.Date = value;
                RaisePropertyChanged();
            }
        }


       public ObservableCollection<Transaction> Transactions
        {
            get { return _wallet.Transactions; }
            set
            {
                _wallet.Transactions = value;
                RaisePropertyChanged(nameof(Transactions));
            }
        }

       public int BeginningOfTransactionList
       {
           get { return _beginningOfTransactionList; }
           set { _beginningOfTransactionList = value; }
       }

       public ObservableCollection<Transaction> TenTransactions
       {
           get { return _wallet.ShowTenTransactions(BeginningOfTransactionList); ; }
           set {/* _tenTransactions = value;*/ }
       }




       public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
            _walService = new WalletService();
            _catService = new CategoryService();
            _tranService = new TransactionService();
            
            WalletCategoriesAdded = new ObservableCollection<Category>();
            WalletCategoriesAvailable = new ObservableCollection<Category>();
            UpdateListsOfCategories();
            
            _usdAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.USD), 2);
            _eurAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.EUR), 2);
            _uahAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.UAH), 2);
            _rubAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.RUB), 2);

            _addTransaction = new Transaction(_wallet.Guid,0.0m,Currency.USD, null,"",DateTime.Now, Guid.NewGuid());

            SubmitChangesCommand = new DelegateCommand(SubmitChanges);
            AddWalletCategoryCommand = new DelegateCommand(AddWalletCategory);
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            DeleteTransactionCommand = new DelegateCommand(DeleteTransaction);
            EditTransactionCommand = new DelegateCommand(EditTransaction);
            ShowTransactionsCommand = new DelegateCommand(ShowTenTransactions);
        }


        public DelegateCommand SubmitChangesCommand { get; }
        public DelegateCommand AddWalletCategoryCommand { get; }
        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }
        public DelegateCommand EditTransactionCommand { get; }
        public DelegateCommand ShowTransactionsCommand { get; }


        //Edit wallet method
        public async void SubmitChanges()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(_wallet.Name))
                {
                    MessageBox.Show("Please,set the name of wallet for submitting changes.");
                    return;
                }
                IsEnabled = false;
                await Task.Run(() => WalletsViewModel._service.AddOrUpdateWalletAsync(_wallet)); //updating the wallet in database
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Changing wallet details failed {ex.Message}");
                return;
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }

            RaisePropertyChanged(nameof(DisplayName));
            RaisePropertyChanged(nameof(LastMonthIncome));
            RaisePropertyChanged(nameof(LastMonthExpense));
            return;
        }


        // This method checks what categories are added in the wallet and what are available
        public void UpdateListsOfCategories()
        {
          WalletCategoriesAdded.Clear();
          WalletCategoriesAvailable.Clear();
          //MessageBox.Show(_walService.GetWallet(FromWallet).Categories.Count.ToString());

          foreach (Category category in _catService.GetCategories())
            {
                if (category.UserGuid == MainInfo.User.Guid)
                {
                    if ( _walService.GetWallet(FromWallet).Categories.ToList().Exists(x =>  x.Guid == category.Guid))
                    {
                        WalletCategoriesAdded.Add(category);
                    }
                    else
                    {
                        WalletCategoriesAvailable.Add(category);
                    }
                }
            }

          return;
        }

        public void UpdateTransactionsCollection()
        {
            Transactions.Clear();
            foreach (Transaction transaction in _tranService.GetTransactions())
            {
                if (transaction.WalletGuid == _wallet.Guid)
                {
                    if (_walService.GetWallet(FromWallet).Transactions.ToList().Exists(x => x.Guid == transaction.Guid ))
                    {
                        Transactions.Add(transaction);
                    }
                }
            }
            RaisePropertyChanged(nameof(Transactions));
        }


        public async void AddWalletCategory()
        {
            try
            {
                if (SelectedCategory == null)
                {
                    MessageBox.Show("Please,select the category.");
                    return;
                }
                IsEnabled = false;
                
                FromWallet.Categories.Add(SelectedCategory);
                await Task.Run(()=>_walService.AddOrUpdateWalletAsync(FromWallet));
               
                UpdateListsOfCategories();
                MessageBox.Show("Adding wallet category completed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adding wallet category failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }

        }



        public async void AddTransaction()
        {
            try
            {
                if (_addTransaction.MoneyAmount == 0.0m )//|| _addTransaction.Category == null || _addTransaction.Currency == null)
                {
                   // MessageBox.Show("Please, choose money amount,category and currency");
                    MessageBox.Show("Please, choose money amount");
                    return;
                }

                if (_addTransaction.Category == null)
                {
                    MessageBox.Show("Please, choose category");
                    return;
                }

                if (_addTransaction.Currency == null)
                {
                    MessageBox.Show("Please, choose currency");
                    return;
                }
                if (_addTransaction.Date == null)
                {
                    MessageBox.Show("Please, choose date");
                    return;
                }
                if (DateTime.Compare(_addTransaction.Date.Value, DateTime.Today.AddDays(1))>0)
                {
                    MessageBox.Show($" {_addTransaction.Date.Value} - Please, don`t look in the future...");
                    return;
                }

                IsEnabled = false;


                _addTransaction.Guid = Guid.NewGuid();
                _wallet.AddTransaction(MainInfo.User,_addTransaction);
                //TODO: MAKE METHOD FOR UPDATE AFTER TRANSACTION

                await Task.Run(() => _walService.AddOrUpdateWalletAsync(_wallet));
                await Task.Run(() => _tranService.AddOrUpdateTransactionAsync(_addTransaction));
                MessageBox.Show("Adding wallet transaction completed!");
                UpdateTransactionsCollection();

                RaisePropertyChanged(nameof(DisplayName));
                RaisePropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(TenTransactions));
                RaisePropertyChanged(nameof(LastMonthIncome));
                RaisePropertyChanged(nameof(LastMonthExpense));
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Adding wallet transaction failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }

        }

        public async void DeleteTransaction()
        {
            try
            {
                IsEnabled = false;
                if (CurrentTransaction == null)
                {
                    MessageBox.Show("Please,choose the transaction.");
                    return;
                }

                await Task.Run(() => _tranService.DeleteTransaction(_currentTransaction));
                _wallet.DeleteTransaction(MainInfo.User,_uneditedTransaction);
                await Task.Run(() => _walService.AddOrUpdateWalletAsync(_wallet));

                MessageBox.Show("Deleting wallet transaction completed!");
                UpdateTransactionsCollection();

                RaisePropertyChanged(nameof(DisplayName));
                RaisePropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(TenTransactions));
                RaisePropertyChanged(nameof(LastMonthIncome));
                RaisePropertyChanged(nameof(LastMonthExpense));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deleting wallet transaction failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }
          
        }


        public async void EditTransaction()
        {
            try
            {
                if (CurrentTransaction == null)
                {
                    MessageBox.Show("Please,choose the transaction.");
                    return;
                }
                if (_currentTransaction.MoneyAmount == 0.0m) 
                {
                    MessageBox.Show("Please,choose the transaction.");
                    return;
                }
                if (_currentTransaction.Category == null)
                {
                    MessageBox.Show("Please,choose the category.");
                    return;
                }
                if (_currentTransaction.Currency == null)
                {
                    MessageBox.Show("Please,choose the currency.");
                    return;
                }
                if (DateTime.Compare(CurrentTransaction.Date.Value, DateTime.Today.AddDays(1)) > 0)
                {
                    MessageBox.Show($" {CurrentTransaction.Date.Value} -  Please, don`t look in the future...");
                    return;
                }

                IsEnabled = false;
                _wallet.EditTransaction(MainInfo.User, _uneditedTransaction);

                await Task.Run(() => _tranService.AddOrUpdateTransactionAsync(_currentTransaction));
                await Task.Run(() => _walService.AddOrUpdateWalletAsync(_wallet));
               
                MessageBox.Show("Editing wallet transaction completed!");
                UpdateTransactionsCollection();

                RaisePropertyChanged(nameof(DisplayName));
                RaisePropertyChanged(nameof(Transactions));
                RaisePropertyChanged(nameof(TenTransactions));
                RaisePropertyChanged(nameof(LastMonthIncome));
                RaisePropertyChanged(nameof(LastMonthExpense));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Editing wallet transaction failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }

        }



        public void ShowTenTransactions()
        {
            if (BeginningOfTransactionList < 0)
            {
                MessageBox.Show("Please, enter the non-negative number.");
            }
            RaisePropertyChanged(nameof(TenTransactions));
        }






       
    }
}
