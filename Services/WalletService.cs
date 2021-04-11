﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallets.BusinessLayer;
using Wallets.DataStorage;



namespace Wallets.Services
{
    public class WalletService
    {
        private FileDataStorage<Wallet> _storage = new FileDataStorage<Wallet>();


        public void DeleteWallet(Wallet wallet)
        {
            Thread.Sleep(1000);
            _storage.Delete(wallet);
        }


        public List<Wallet> GetWallets()
        {
            Task<List<Wallet>> wallets = Task.Run<List<Wallet>>(async () => await _storage.GetAllAsync());
            return wallets.Result;
        }
        public async Task<bool> AddOrUpdateWalletAsync(Wallet wallet)
        {
            Thread.Sleep(1000);
            await Task.Run(() => _storage.AddOrUpdateAsync(wallet));
            return true;
        }




    }
}
