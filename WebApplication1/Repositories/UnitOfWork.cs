
using Shop.ProductAPI.Context;

namespace Shop.ProductAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ??  new CategoryRepository(_context);  } 
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository ?? new ProductRepository(_context); }
        }

        private readonly ApplicationDbContext _context;


        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
               
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
