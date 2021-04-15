using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wallets.BusinessLayer;
using Wallets.DataStorage;
using Wallets.Models.Users;

namespace Wallets.Services
{
    public class AuthenticationService
    {
        private FileDataStorage<DbUser> _storage = new FileDataStorage<DbUser>();
        
        
        public async Task<Client> Authenticate(AuthenticationUser authUser)
        {
            Thread.Sleep(2000);
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
            {
                throw new ArgumentException("Login or Password is empty.");
            }

            var users = await Task.Run( ()=>_storage.GetAllAsync());

            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == Encrypt(authUser.Password));
            if (dbUser == null)
                throw new Exception("Wrong login or password.");

            Client curUser= new Client(dbUser.Guid, dbUser.LastName, dbUser.FirstName, dbUser.Email, dbUser.Login);
            MainInfo.User = curUser;
            return curUser;
        }

        public async Task<bool> RegistrateUser(RegistrationUser regUser)
        {
            Thread.Sleep(2000);

            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User with this login already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password)
                                                         || String.IsNullOrWhiteSpace(regUser.LastName)
                                                         || String.IsNullOrWhiteSpace(regUser.FirstName)
                                                         || String.IsNullOrWhiteSpace(regUser.Email))
                throw new ArgumentException("Some of fields are empty.");

            if(!Regex.IsMatch(regUser.Email, @"[a-zA-Z0-9]+@[a-z]+(\.)[a-z]+$")) // regex for email
                throw new ArgumentException("Invalid email");

            if(regUser.Login.Length>30 || regUser.Password.Length>30 || regUser.LastName.Length>30 || regUser.FirstName.Length>30)
                throw new ArgumentException("All the fields should be not more than 30 symbols");

            if (regUser.Login.Length < 3 || regUser.Password.Length < 3 || regUser.LastName.Length < 3 || regUser.FirstName.Length <3)
                throw new ArgumentException("All the fields should be not less than 3 symbols");


            dbUser = new DbUser(Guid.NewGuid(), regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, Encrypt(regUser.Password));
            await _storage.AddOrUpdateAsync(dbUser);

            return true;
        }

        private string Encrypt(string value) //FOR PASSWORD:
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }



    }
}
