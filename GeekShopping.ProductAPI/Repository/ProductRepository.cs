using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository {
    public class ProductRepository : IProductRepository {

        private readonly MariaDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(MariaDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> FindAll() {
            return _mapper.Map<List<ProductVO>>(await _context.Products.ToListAsync());
        }

        public async Task<ProductVO> FindById(long id) {
            return _mapper.Map<ProductVO>(await _context.Products.FirstOrDefaultAsync(product => product.Id == id));
        }

        public async Task<ProductVO> Create(ProductVO productVO) {
            Product product = _mapper.Map<Product>(productVO);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Update(ProductVO productVO) {
            Product product = _mapper.Map<Product>(productVO);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<bool> Delete(long id) {
            try {
                Product product = _mapper.Map<Product>(await FindById(id));
                if (product == null) return false;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception) {

                return false;
            }
        }


    }
}
