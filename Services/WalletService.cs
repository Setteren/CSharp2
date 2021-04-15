using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Wallets.BusinessLayer;
using Wallets.DataStorage;
using Wallets.Models.Users;


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

        public async Task<bool> AddOrUpdateWalletAsync(Wallet wallet)
        {
            Thread.Sleep(1000);
            await Task.Run( ()=> _storage.AddOrUpdateAsync(wallet));
            return true;
        }


     
        public Wallet GetWallet(Wallet wallet)
        {
            Task<Wallet> walresult = Task.Run<Wallet>(async () => await _storage.GetAsync(wallet.Guid));
            return walresult.Result;
        }


        public List<Wallet> GetWallets()
        { 
            Task<List<Wallet>> wallets = Task.Run<List<Wallet>>(async () => await _storage.GetAllAsync());
            return wallets.Result;
        }

    }
}
