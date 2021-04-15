using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Wallets.BusinessLayer;
using Wallets.GUI.WPF.Navigation;
using Wallets.Services;

namespace Wallets.GUI.WPF.Wall_Categories
{
    public class AddUserCategoryViewModel: BindableBase, INavigatable<MainNavigatableTypes>
    {
        private CategoryService _service;

        private Category _category;
        private Action _goToWallets;
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


        public string Name
        {
            get
            {
                return _category.Name;
            }
            set
            {
                _category.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get
            {
                return _category.Description;
            }
            set
            {
                _category.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public Color Color
        {
            get
            {
                return _category.ColorWrapper.Color;
            }
            set
            {
                _category.ColorWrapper.Color = value;
                RaisePropertyChanged(nameof(Color));
            }
        }

        public System.Drawing.Icon Image
        {
            get
            {
                return _category.Image;
            }
            set
            {
                _category.Image = value;
                RaisePropertyChanged(nameof(Image));
            }
        }
        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.AddCategory;
            }
        }

        public DelegateCommand GoToWalletsViewCommand { get; }
        public DelegateCommand AddUserCategoryCommand { get; }


        public AddUserCategoryViewModel(Action goToWallets)
        {
            _service = new CategoryService();
            _category = new Category(Guid.Empty, "", "", null, null,Guid.Empty);
            _goToWallets = goToWallets;
            GoToWalletsViewCommand = new DelegateCommand(_goToWallets);
            AddUserCategoryCommand = new DelegateCommand(AddCategory);


        }


        public void AddCategory()
        {
            try
            {
                IsEnabled = false;
                if (String.IsNullOrWhiteSpace(_category.Name))
                {
                    MessageBox.Show("Name of category is empty.");
                    return;
                }

                Category addCategory = new Category(MainInfo.User.Guid, _category.Name, _category.Description, _category.ColorWrapper, _category.Image,Guid.NewGuid());
                Task.Run(() => _service.AddOrUpdateCategoryAsync(addCategory));
                MainInfo.User.Categories.Add(addCategory);
                MessageBox.Show($"Category {addCategory.Name} added to the list of categories!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adding category failed {ex.Message}");
            }
            finally // is done independently from exception
            {
                IsEnabled = true;
            }


        }




        
        public void ClearSensitiveData()
        {
            _category.Name = "";
            _category.Description = "";
            _category.ColorWrapper = new ColorWrapper(Color.White);
            _category.Image = null;
        }
    }
}
