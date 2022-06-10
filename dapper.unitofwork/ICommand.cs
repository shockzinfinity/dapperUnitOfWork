using System.Data;

namespace dapper.unitofwork;

public interface ICommand
{
  bool RequiresTransaction { get; }

  void Execute(IDbConnection connection, IDbTransaction transaction);
}

public interface ICommand<out T>
{
  bool RequiresTransaction { get; }

  T Execute(IDbConnection connection, IDbTransaction transaction);
}

public interface IAsyncCommand
{
  bool RequiresTransaction { get; }

  Task ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
}

public interface IAsyncCommand<T>
{
  bool RequiresTransaction { get; }

  Task<T> ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default);
}