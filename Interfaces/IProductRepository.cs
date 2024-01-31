using MasteryTest3.Models;

namespace MasteryTest3.Interfaces;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById<T>(T id);
    int AddProduct(Product product);
    int UpdateProduct<T>(T id, Product product);
    int DeleteProduct<T>(T id);
}