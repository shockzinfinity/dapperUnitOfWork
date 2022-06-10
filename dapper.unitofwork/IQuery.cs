using System.Data;

namespace dapper.unitofwork;

public interface IQuery<out T>
{
  T Execute(IDbConnection connection, IDbTransaction transaction);
}

public interface IAsyncQuery<T>
{
  Task<T> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
}