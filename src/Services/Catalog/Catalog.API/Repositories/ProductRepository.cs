using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogcontext;
        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogcontext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }
        public async Task CreateProductAsync(Product product)
        {
            await _catalogcontext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _catalogcontext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _catalogcontext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string category)
        {
            // Eq means Like in SQl. ElemMatch has a lot of timeout but ideal to use.

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _catalogcontext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _catalogcontext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _catalogcontext.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            // Mongo DB store data as Json fomart so we will replace the Json rather than update.

            var updateProduct = await _catalogcontext.Products
                .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }
    }
}
