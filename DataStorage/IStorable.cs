using System;

namespace Wallets.DataStorage
{
    public interface IStorable
    {
        Guid Guid { get; }
    }
}
