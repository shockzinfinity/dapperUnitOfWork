using dapper.unitofwork;
using Dapper;
using System.Data;

namespace dapper.testconsole;

public class DeleteProductCommand : ICommand
{
  private const string _sql = @"
DELETE Products WHERE Id = @Id
";

  private readonly string _productId;

  public DeleteProductCommand(string productId) => _productId = productId;

  public bool RequiresTransaction => false;

  public void Execute(IDbConnection connection, IDbTransaction transaction) =>
    connection.Execute(_sql, new {
      Id = _productId
    }, transaction);
}