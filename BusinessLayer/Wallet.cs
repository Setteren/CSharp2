using System;
using System.Collections.Generic;
using Wallets.DataStorage;

namespace Wallets.BusinessLayer
{
    public class Wallet : EntityBase, IStorable

    {
    private static int _instanceCountWallets = 0;

    private Guid _guid;
    private string _name;
    private decimal _startingBalance;
    private string _description;
    private Currency? _mainCurrency;
    private List<Transaction> _transactions;
    private List<Category> _categories;
    private decimal _balance;

    private Guid _ownerGuid;
    private List<Guid> _coOwnersGuid;

    public Guid Guid
    {
        get => _guid;
        set => _guid = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public decimal StartingBalance
    {
        get => _startingBalance;
        private set => _startingBalance = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public Currency? MainCurrency
    {
        get => _mainCurrency;
        set => _mainCurrency = value;
    }

    public List<Transaction> Transactions
    {
        get
        {
            List<Transaction> result = new List<Transaction>();
            foreach (Transaction listTransaction in _transactions)
            {
                result.Add(listTransaction);

            }

            return result;
        }
        set => _transactions = value;
    }

    public List<Category> Categories
    {
        get => _categories; /*set => _categories = value;*/
    }

    public decimal Balance
    {
        get => _balance;
        set => _balance = value;
    }

    public Guid OwnerGuid
    {
        get => _ownerGuid;
        set => _ownerGuid = value;
    }

    public List<Guid> CoOwnersGuid
    {
        get => _coOwnersGuid;
    }

    public Wallet(Guid ownerGuid, string name, decimal startingBalance, string description, Currency? mainCurrency)
    {
        _guid = Guid.NewGuid();
        _ownerGuid = ownerGuid;
        _name = name;
        _startingBalance = startingBalance;
        _description = description;
        _mainCurrency = mainCurrency;
        _balance = startingBalance;
        _transactions = new List<Transaction>();
        _categories = new List<Category>();
        _coOwnersGuid = new List<Guid>();
    }

    public void AddTransaction(Client user, decimal moneyAmount, Currency currency, Category category, string description,
        List<string> files, DateTime date)
    {
        if (OwnerGuid == user.Guid || CoOwnersGuid.Exists(x => x == user.Guid))
        {
            if (category.UserGuid == OwnerGuid)
            {
                Balance += moneyAmount * Converter.СomputeTheCoefficient(currency, MainCurrency);
                var newTransaction = new Transaction(Guid, moneyAmount, currency, category, description, files, date);
                var temp = Transactions;
                temp.Add(newTransaction);
                Transactions = temp;
            }
        }

        return;
    }

    public void EditTransaction(Client user, Transaction transaction, decimal moneyAmount, Currency currency,
        Category category, string description, DateTime date)
    {
        if (OwnerGuid == user.Guid && category.UserGuid == user.Guid)
        {
            foreach (Transaction listTransaction in Transactions)
            {
                if (listTransaction.Guid == transaction.Guid)
                {
                    Balance -= listTransaction.MoneyAmount *
                               Converter.СomputeTheCoefficient(listTransaction.Currency, MainCurrency);

                    listTransaction.MoneyAmount = moneyAmount;
                    listTransaction.Currency = currency;
                    listTransaction.Category = category;
                    listTransaction.Description = description;
                    listTransaction.Date = date;
                    Balance += listTransaction.MoneyAmount *
                               Converter.СomputeTheCoefficient(listTransaction.Currency, MainCurrency);
                    return;
                }
            }
        }

        return;
    }

    public void DeleteTransaction(Client user, Transaction transaction)
    {
        if (OwnerGuid == user.Guid)
        {
            foreach (Transaction listTransaction in Transactions)
            {
                if (listTransaction.Guid == transaction.Guid)
                {
                    Balance -= listTransaction.MoneyAmount *
                               Converter.СomputeTheCoefficient(listTransaction.Currency, MainCurrency);
                    var temp = Transactions;
                    temp.Remove(listTransaction);
                    Transactions = temp;
                    return;
                }
            }
        }

        return;
    }


    public decimal LastMonthIncome()
    {
        return LastMonth(true);
    }

    public decimal LastMonthExpense()
    {
        return LastMonth(false);
    }

    private decimal LastMonth(bool positive)
    {
        decimal result = 0.0m;
        DateTime currentDate = DateTime.UtcNow;

        foreach (Transaction listTransaction in Transactions)
        {
            if (DateTime.Compare(listTransaction.Date.AddMonths(1), currentDate) > 0) //TODO: check this out
            {
                if ((listTransaction.MoneyAmount > 0 && positive) || (listTransaction.MoneyAmount < 0 && !positive))
                {
                    result += listTransaction.MoneyAmount *
                              Converter.СomputeTheCoefficient(listTransaction.Currency, MainCurrency);
                }
            }
        }

        return result;
    }


    public List<Transaction> ShowTenTransactions(int number)
    {
        _transactions.Sort((x, y) => x.Date.CompareTo(y.Date));

        List<Transaction> result = new List<Transaction>();
        int transactionsShown = 10;

        if (number + transactionsShown > Transactions.Count)
        {
            number = Transactions.Count - transactionsShown;
        }

        if (transactionsShown > Transactions.Count)
        {
            number = 0;
            transactionsShown = Transactions.Count;
        }

        for (int i = number; i < number + transactionsShown; i++)
        {
            result.Add(Transactions[i]);
        }

        return result;
    }

    public override bool Validate()
    {
        var result = true;

        if (OwnerGuid == Guid.Empty)
            result = false;
        if (String.IsNullOrWhiteSpace(Name))
            result = false;
        if (StartingBalance < 0)
            result = false;
        if (MainCurrency == null)
        {
            result = false;
        }

        return result;
    }










    }
}
