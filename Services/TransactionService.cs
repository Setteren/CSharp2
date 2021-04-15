using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wallets.BusinessLayer;
using Wallets.DataStorage;

namespace Wallets.Services
{
    public class TransactionService
    {
        private FileDataStorage<Transaction> _storage = new FileDataStorage<Transaction>();


        public void DeleteTransaction(Transaction transaction)
        {
            Thread.Sleep(1000);
            _storage.Delete(transaction);
        }


        public async Task<bool> AddOrUpdateTransactionAsync(Transaction transaction)
        {
            Thread.Sleep(1000);
            await Task.Run(() => _storage.AddOrUpdateAsync(transaction));
            return true;
        }


    
        public List<Transaction> GetTransactions()
        {
            Task<List<Transaction>> transactions = Task.Run<List<Transaction>>(async () => await _storage.GetAllAsync());
            return transactions.Result;
        }

    }
}
