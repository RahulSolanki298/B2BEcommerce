using Core.IRepository;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ImageUploadByZipFile()
        {


            return false;
        }


    }
}
