namespace dapper.unitofwork;

public interface IUnitOfWork : IDisposable
{
  T Query<T>(IQuery<T> query);

  Task<T> QueryAsync<T>(IAsyncQuery<T> query);

  Task<T> QueryAsync<T>(IAsyncQuery<T> query, CancellationToken cancellationToken);

  void Execute(ICommand command);

  Task ExecuteAsync(IAsyncCommand command);

  Task ExecuteAsync(IAsyncCommand command, CancellationToken cancellationToken);

  T Execute<T>(ICommand<T> command);

  Task<T> ExecuteAsync<T>(IAsyncCommand<T> command);

  Task<T> ExecuteAsync<T>(IAsyncCommand<T> command, CancellationToken cancellationToken);

  void Commit();

  void Rollback();
}