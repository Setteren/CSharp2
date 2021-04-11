using System;
using System.Collections.Generic;
using System.Drawing;


namespace Wallets.BusinessLayer
{
    public class Client : EntityBase
    {

        private Guid _guid;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _login;

        private List<Wallet> _wallets;
        private List<Category> _categories;
        private List<Wallet> _walletsCoOwnered;

        public Guid Guid { get => _guid; private set => _guid = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Email { get => _email; set => _email = value; }
        public string Login { get => _login; set => _login = value; }

        public List<Wallet> Wallets { get => _wallets; }
        public List<Category> Categories { get => _categories; /*set => _categories = value;*/ }
        public List<Wallet> WalletsCoOwnered { get => _walletsCoOwnered;/* set => _walletsCoOwnered = value;*/ }


        public Client()
        {
            _wallets = new List<Wallet>();
            _categories = new List<Category>();
            _walletsCoOwnered = new List<Wallet>();
        }

        public Client(Guid guid, string lastName, string firstName, string email, string login) : this()
        {
            _guid = guid;
            _lastName = lastName;
            _firstName = firstName;
            _email = email;
            _login = login;
        }


        public string FullName
        {
            get
            {
                var result = LastName;
                if (!String.IsNullOrWhiteSpace(FirstName))
                {
                    if (!String.IsNullOrWhiteSpace(LastName))
                    {
                        result += ", ";
                    }
                    result += FirstName;
                }
                return result;
            }
        }

        public void AddWallet(string name, decimal startingBalance, string description, Currency mainCurrency)
        {
            var newWallet = new Wallet(Guid, name, startingBalance, description, mainCurrency);
            Wallets.Add(newWallet);
        }

        public void AddCategory(string name, string description, Color color, Icon icon)
        {
            if (!Categories.Exists(x => x.Name == name && x.Description == description))
            {
                var newCategory = new Category(Guid, name, description, color, icon);
                Categories.Add(newCategory);
            }
        }

        public void AddWalletCategory(Wallet wallet, Category category)
        {
            if (wallet.OwnerGuid == Guid)
            {
                if (category.UserGuid == Guid)
                {
                    if (!wallet.Categories.Exists(x => x.Name == category.Name && x.Description == category.Description))
                    {
                        wallet.Categories.Add(category);
                    }
                }
            }
        }

        public void ShareWallet(Client user, Wallet wallet)
        {
            if (wallet.OwnerGuid == Guid && Guid != user.Guid)
            {
                if (!wallet.CoOwnersGuid.Exists(x => x == user.Guid))
                {
                    wallet.CoOwnersGuid.Add(user.Guid);
                    user.WalletsCoOwnered.Add(wallet);
                }
            }
        }





        public override bool Validate()
        {
            var result = true;

            if (String.IsNullOrWhiteSpace(Email))
                result = false;
            if (String.IsNullOrWhiteSpace(Login))
                result = false;

            return result;
        }

        public override string ToString()
        {
            return FullName;
        }

    }
}
