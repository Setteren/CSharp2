using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using Wallets.BusinessLayer;

namespace Wallets.GUI.WPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
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
                //    RaisePropertyChanged(nameof(DisplayName));
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
                // TbBalance.Text = value;
                //    RaisePropertyChanged(nameof(DisplayName));
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
                //    RaisePropertyChanged(nameof(DisplayName));
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
                //  RaisePropertyChanged(nameof(DisplayName));
            }
        }


        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name} ({_wallet.Balance} {_wallet.MainCurrency})";
            }
        }



        public WalletDetailsViewModel(Wallet wallet)
        {
            _wallet = wallet;
            _usdAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.USD), 2);
            _eurAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.EUR), 2);
            _uahAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.UAH), 2);
            _rubAmount = Math.Round(_wallet.Balance * Converter.СomputeTheCoefficient(_wallet.MainCurrency, Currency.RUB), 2);
            SubmitChangesCommand = new DelegateCommand(SubmitChanges);
        }


        public DelegateCommand SubmitChangesCommand { get; }


        //TODO: make submitting changes async 
        public async void SubmitChanges()
        {
            try
            {
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
            return;
        }







    }
}
