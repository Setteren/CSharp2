
using System;

namespace Wallets.GUI.WPF.Navigation
{




    public interface INavigatable<TOBject> where TOBject : Enum
    {
        public TOBject Type { get; }
        public void ClearSensitiveData();
    }



}
