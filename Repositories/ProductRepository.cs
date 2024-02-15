using System.Data;
using Dapper;
using MasteryTest3.Interfaces;
using MasteryTest3.Models;

namespace MasteryTest3.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnection _connection;

    public ProductRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _connection.Query<Product, UOM, Product>(
            "GetProductList",
            (product, uom)=>{
                product.uom = uom;
                return product;
            },
            splitOn:"Id");
    }

    public int AddProduct(Product product)
    {
        return _connection.Execute(
            "sp_save_product", 
            new
            {
                product.name,
                product.description,
                product.photo,
                product.sku,
                product.size,
                product.color,
                categoryId = product.category.Id,
                product.weight,
                product.price
            },
            commandType: CommandType.StoredProcedure
        ); 
    }

    public int UpdateProduct<T>(T id, Product product)
    {
        return _connection.Execute(
            "sp_save_product", 
            new
            {
                id,
                product.name,
                product.description,
                product.photo,
                product.sku,
                product.size,
                product.color,
                categoryId = product.category.Id,
                product.weight,
                product.price
            },
            commandType: CommandType.StoredProcedure
        );
    }

    public int DeleteProduct<T>(T id)
    {
        return _connection.Execute(
            "sp_delete_product", 
            new { Id = id },
            commandType: CommandType.StoredProcedure
        ); 
    }
}