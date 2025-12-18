namespace Shop.ProductAPI.Repositories
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }

        Task Save();

    }
}
