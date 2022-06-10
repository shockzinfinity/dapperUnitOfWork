using dapper.unitofwork;
using Dapper;
using System.Data;

namespace dapper.testconsole;

public class CreateTableCommand : ICommand
{
  private readonly string _sql;

  public CreateTableCommand(string sql) => _sql = sql;

  public bool RequiresTransaction => false;

  public void Execute(IDbConnection connection, IDbTransaction transaction) =>
    connection.Execute(_sql, transaction);
}