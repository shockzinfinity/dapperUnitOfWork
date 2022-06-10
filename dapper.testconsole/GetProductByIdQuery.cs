using dapper.unitofwork;
using Dapper;
using System.Data;

namespace dapper.testconsole
{
  public class GetProductByIdQuery : IQuery<ProductEntity>, IAsyncQuery<ProductEntity>
  {
    private const string _sql = @"
SELECT
  Id,
  ProductName,
  GnvProductCode,
  ManufactureDate
FROM Products
WHERE Id = @Id
";

    private readonly string _productId;

    public GetProductByIdQuery(string productId) => _productId = productId;

    public ProductEntity Execute(IDbConnection connection, IDbTransaction transaction) =>
      connection.Query<ProductEntity>(_sql, new {
        Id = Guid.Parse(_productId)
      }, transaction).FirstOrDefault();

    public Task<ProductEntity> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default) =>
      connection.QueryFirstOrDefaultAsync<ProductEntity>(_sql, new {
        Id = Guid.Parse(_productId)
      }, transaction);
  }
}