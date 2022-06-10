using System.Data;

namespace dapper.unitofwork;

public class UnitOfWork : IUnitOfWork
{
  private bool _disposed;
  private IDbConnection? _connection;
  private readonly RetryOptions? _retryOptions;
  private readonly CancellationToken _cancellationToken;
  private IDbTransaction? _transaction = null;

  public UnitOfWork(IDbConnection connection, bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions? retryOptions = null, CancellationToken cancellationToken = default)
  {
    _connection = connection;
    _retryOptions = retryOptions;
    _cancellationToken = cancellationToken;

    if (transactional)
      _transaction = connection.BeginTransaction(isolationLevel);
  }

  public T Query<T>(IQuery<T> query) => Retry.Do(() => query.Execute(_connection!, _transaction!), _retryOptions!);

  public Task<T> QueryAsync<T>(IAsyncQuery<T> query)
    => Retry.DoAsync(() => query.ExecuteAsync(_connection!, _transaction!, _cancellationToken), _retryOptions!);

  public Task<T> QueryAsync<T>(IAsyncQuery<T> query, CancellationToken cancellationToken)
    => Retry.DoAsync(() => query.ExecuteAsync(_connection!, _transaction!, cancellationToken), _retryOptions!);

  public void Execute(ICommand command)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    Retry.Do(() => command.Execute(_connection!, _transaction!), _retryOptions!);
  }

  public T Execute<T>(ICommand<T> command)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    return Retry.Do(() => command.Execute(_connection!, _transaction!), _retryOptions!);
  }

  public Task ExecuteAsync(IAsyncCommand command)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    return Retry.DoAsync(() => command.ExecuteAsync(_connection!, _transaction!, _cancellationToken), _retryOptions!);
  }

  public Task ExecuteAsync(IAsyncCommand command, CancellationToken cancellationToken)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    return Retry.DoAsync(() => command.ExecuteAsync(_connection!, _transaction!, cancellationToken), _retryOptions!);
  }

  public Task<T> ExecuteAsync<T>(IAsyncCommand<T> command)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    return Retry.DoAsync(() => command.ExecuteAsync(_connection!, _transaction!, _cancellationToken), _retryOptions!);
  }

  public Task<T> ExecuteAsync<T>(IAsyncCommand<T> command, CancellationToken cancellationToken)
  {
    if (command.RequiresTransaction && _transaction == null)
      throw new Exception($"This command {command.GetType()} requires a transaction");

    return Retry.DoAsync(() => command.ExecuteAsync(_connection!, _transaction!, cancellationToken), _retryOptions!);
  }

  public void Commit() => _transaction?.Commit();

  public void Rollback() => _transaction?.Rollback();

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  ~UnitOfWork() => Dispose(false);

  protected virtual void Dispose(bool disposing)
  {
    if (_disposed)
      return;

    if (disposing) {
      _transaction?.Dispose();
      _connection?.Dispose();
    }

    _transaction = null;
    _connection = null;

    _disposed = true;
  }
}