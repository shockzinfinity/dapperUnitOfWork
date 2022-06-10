namespace dapper.unitofwork;

public interface IExceptionDetector
{
  bool ShouldRetryOn(Exception ex);
}