namespace Wallets.BusinessLayer
{
    public abstract class EntityBase
    {
        public bool IsValid => Validate();

        public abstract bool Validate();

    }
}
