
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using Wallets.GUI.WPF.Authentication;
using Wallets.GUI.WPF.Wallets;

namespace Wallets.GUI.WPF.Navigation
{
    public abstract class NavigationBase<TObject> : BindableBase where TObject: Enum
    {




        private List<INavigatable<TObject>> _viewModels = new List<INavigatable<TObject>>();
        private Action _signInSuccess;



        public INavigatable<TObject> CurrentViewModel
        {
            get;
            private set;
        }

        protected NavigationBase()
        {
        }

        public void Navigate(TObject type)
        {
            if (CurrentViewModel != null && CurrentViewModel.Type.Equals(type))
                return;
            INavigatable<TObject> viewModel = _viewModels.FirstOrDefault(navigatable => navigatable.Type.Equals(type));

            if (viewModel == null)
            {
                viewModel = CreateViewModel(type);
                _viewModels.Add(viewModel);
            }

            viewModel.ClearSensitiveData();
            CurrentViewModel = viewModel;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }


        protected abstract INavigatable<TObject> CreateViewModel(TObject type);

    }

}
