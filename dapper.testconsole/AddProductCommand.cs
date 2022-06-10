using dapper.unitofwork;
using Dapper;
using System.Data;

namespace dapper.testconsole;

public class AddProductCommand : ICommand, IAsyncCommand
{
  private const string _sql = @"
INSERT INTO Products (Id, ProductName, GnvProductCode, ManufactureDate, UnitPrice, CreatedAt, UpdateAt) VALUES (@Id, @ProductName, @GnvProductCode, @ManufactureDate, @UnitPrice, @CreatedAt, @UpdateAt)
";

  private readonly ProductEntity _productEntity;

  public bool RequiresTransaction => false;

  public AddProductCommand(ProductEntity productEntity) => _productEntity = productEntity;

  public void Execute(IDbConnection connection, IDbTransaction transaction) =>
    connection.Execute(_sql, new {
      Id = _productEntity.Id,
      ProductName = _productEntity.ProductName,
      GnvProductCode = _productEntity.GnvProductCode,
      ManufactureDate = _productEntity.ManufactureDate,
      UnitPrice = _productEntity.UnitPrice,
      CreatedAt = DateTime.Now,
      UpdateAt = DateTime.Now
    }, transaction);

  public Task ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default) =>
    connection.ExecuteAsync(_sql, new {
      id = _productEntity.Id,
      ProductName = _productEntity.ProductName,
      GnvProductCode = _productEntity.GnvProductCode,
      ManufactureDate = _productEntity.ManufactureDate,
      UnitPrice = _productEntity.UnitPrice,
      CreatedAt = DateTime.Now,
      UpdateAt = DateTime.Now
    }, transaction);
}