
using Shop.CartAPI.Context;

namespace Shop.CartAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICartRepository cartRepository;

        public ICartRepository CartRepository 
        {
            get { return cartRepository ?? new CartRepository(_context); }
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
