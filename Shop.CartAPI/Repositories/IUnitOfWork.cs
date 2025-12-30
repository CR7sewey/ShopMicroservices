namespace Shop.CartAPI.Repositories
{
    public interface IUnitOfWork
    {
        public ICartRepository CartRepository { get; }
        Task Save();

    }
}
