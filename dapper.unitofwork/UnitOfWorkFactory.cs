using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dapper.unitofwork;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
  private readonly DbProviderFactory _dbProviderFactory;
  private readonly string _connectionString;

  public UnitOfWorkFactory(string connectionString, DbProviderFactory dbProviderFactory = null)
  {
    _connectionString = connectionString;
    _dbProviderFactory = dbProviderFactory ?? SqlClientFactory.Instance;
  }

  public IUnitOfWork Create(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions retryOptions = null)
  {
    var connection = BuildConnection();
    connection.Open();

    return new UnitOfWork(connection, transactional, isolationLevel, retryOptions);
  }

  public async Task<IUnitOfWork> CreateAsync(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, RetryOptions retryOptions = null, CancellationToken cancellationToken = default)
  {
    var connection = BuildConnection();
    await connection.OpenAsync(cancellationToken);

    return new UnitOfWork(connection, transactional, isolationLevel, retryOptions);
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }

  private DbConnection BuildConnection()
  {
    var connection = _dbProviderFactory.CreateConnection();
    if (connection == null)
      throw new Exception("Error initilizing connection");

    connection.ConnectionString = _connectionString;

    return connection;
  }
}