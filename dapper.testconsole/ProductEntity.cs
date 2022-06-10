namespace dapper.testconsole
{
  public class ProductEntity
  {
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public string GnvProductCode { get; set; }
    public DateTime ManufactureDate { get; set; }
    public decimal UnitPrice { get; set; }
  }
}