using System;
using System.Collections.Generic;

namespace Wallets.BusinessLayer
{
    public class Transaction : EntityBase
    {

        private Guid _guid;
        private decimal _moneyAmount;
        private Currency? _currency;
        private Category _category;
        private string _description;
        private DateTime _date;
        private List<string> _files;
        private Guid _walletGuid;

        public Guid Guid { get => _guid; set => _guid = value; }
        public decimal MoneyAmount { get => _moneyAmount; set => _moneyAmount = value; }
        public Currency? Currency { get => _currency; set => _currency = value; }
        public Category Category { get => _category; set => _category = value; }
        public string Description { get => _description; set => _description = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public Guid WalletGuid { get => _walletGuid; private set => _walletGuid = value; }
        public List<string> Files { get => _files; set => _files = value; }

        public Transaction(Guid walletGuid, decimal moneyAmount, Currency? currency, Category category, string description, List<string> files, DateTime date)
        {
            _guid = Guid.NewGuid();

            _walletGuid = walletGuid;
            _moneyAmount = moneyAmount;
            _currency = currency;
            _category = category;
            _description = description;
            _files = files;
            _date = date;
        }

        public override bool Validate()
        {
            var result = true;

            if (WalletGuid == Guid.Empty)
                result = false;
            if (Currency == null)
                result = false;
            if (Category == null)
                result = false;
            if (MoneyAmount == 0)
                result = false;

            return result;
        }

    }
}
