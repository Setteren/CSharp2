using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Wallets.BusinessLayer;
using Wallets.GUI.WPF.Navigation;
using Wallets.GUI.WPF.Wallets;
using Wallets.Services;

namespace Wallets.GUI.WPF.Wall_Categories
{
    public class DeleteUserCategoryViewModel: BindableBase, INavigatable<MainNavigatableTypes>
    {
        private WalletService _walService;
        private CategoryService _catService;
        private Category _selectedCategory;
        private Action _goToWalletsView;
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


        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(nameof(SelectedCategory));
            }
        }

        public static ObservableCollection<Category> UserCategories { get; set; }

        public DeleteUserCategoryViewModel(Action gotoWalletsView)
        {
            UserCategories = new ObservableCollection<Category>();
            _walService = new WalletService();
            _catService = new CategoryService();
            _goToWalletsView = gotoWalletsView;
            GoToWalletsViewCommand = new DelegateCommand(_goToWalletsView);
            DeleteUserCategoryCommand = new DelegateCommand(DeleteCategory);
        }

        public DelegateCommand GoToWalletsViewCommand { get; }
        public DelegateCommand DeleteUserCategoryCommand { get; }


        private void UpdateUserCategoriesCollection()
        {
            UserCategories.Clear();
            foreach (Category category in _catService.GetCategories())
            {
                if (category.UserGuid == MainInfo.User.Guid)
                {
                    UserCategories.Add(category);
                }
            }
        }


        private void DeleteCategory()
        {

            try
            {
                if (SelectedCategory == null)
                {
                    MessageBox.Show("Please,select the category.");
                    return;
                }
                IsEnabled = false;
                Task.Run(() => _catService.DeleteCategory(SelectedCategory));
                MainInfo.User.Categories.Remove(SelectedCategory);
                
                foreach (Wallet wallet in _walService.GetWallets())
                {
                    if (wallet.OwnerGuid == MainInfo.User.Guid)
                    { 
                        wallet.Categories.Remove(wallet.Categories.ToList().Find(x => x.Name == SelectedCategory.Name && x.Guid == SelectedCategory.Guid));
                        Task.Run(()=>_walService.AddOrUpdateWalletAsync(wallet));
                    }
                }


                MessageBox.Show($"Category {SelectedCategory.Name} removed from the list of categories!");
                UpdateUserCategoriesCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deleting category failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }

        }


        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.DeleteCategory;
            }
        }

        public void ClearSensitiveData()
        {
            SelectedCategory = null;
            UpdateUserCategoriesCollection();
        }

      


    }
}
