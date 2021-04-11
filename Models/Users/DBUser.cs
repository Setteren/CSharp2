using System;
using Wallets.DataStorage;

namespace Wallets.Models.Users
{
    public class DbUser : IStorable
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public Guid Guid { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }


        public DbUser(Guid guid, string firstName, string lastName, string email, string login, string password)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
        }

    }
}
