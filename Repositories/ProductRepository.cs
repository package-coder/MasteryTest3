﻿using System.Data;
using Dapper;
using Microsoft.Identity.Client;
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
        return _connection.Query<Product>("sp_get_products", commandType: CommandType.StoredProcedure);
    }

    public Product? GetProductById<T>(T id)
    {
        var products = _connection.Query<Product?>(
            "sp_get_product_by_id", 
            new { id }
            );
        return products.FirstOrDefault();
    }

    public int AddProduct(Product product)
    {
        return _connection.Execute(
            "sp_save_product", 
            new
            {
                product.name,
                product.description,
                product.urlPhoto,
                product.sku,
                product.size,
                product.color,
                product.categoryId,
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
                product.urlPhoto,
                product.sku,
                product.size,
                product.color,
                product.categoryId,
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