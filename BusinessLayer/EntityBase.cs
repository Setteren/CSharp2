namespace Wallets.BusinessLayer
{
    public abstract class EntityBase
    {
        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        public abstract bool Validate();

    }
}
