using dapper.testconsole;
using dapper.unitofwork;

internal class Program
{
  private const string ConnectionString = @"Data Source=localhost;Database=dapperTest;User id=sa;Password=xxxxx;";

  private static void Main(string[] args)
  {
    var factory = new UnitOfWorkFactory(ConnectionString);

    PrintProduct("655FDFED-0E69-4293-98CC-2CF0C99A00BE");

    var newProductId = Guid.NewGuid();

    using (var uow = factory.Create(true)) {
      uow.Execute(new AddProductCommand(new ProductEntity
      {
        Id = newProductId,
        ProductName = "test product",
        GnvProductCode = "SHOZ-18394",
        ManufactureDate = DateTime.Now,
        UnitPrice = 4.3M
      }));
      uow.Commit();
    }

    PrintProduct(newProductId.ToString("n"));

    Task.Run(() => MainAsync(args)).Wait();

    var createTableString = @"
CREATE TABLE [dbo].[Products2](
	[Id] [uniqueidentifier] NOT NULL,
	[ChargedBy] [nvarchar](max) NULL,
	[ProductName] [nvarchar](max) NOT NULL,
	[Formulas] [nvarchar](max) NULL,
	[ProductCategory] [nvarchar](max) NULL,
	[VolumeUnit] [nvarchar](max) NULL,
	[Volume] [nvarchar](max) NULL,
	[Material] [nvarchar](max) NULL,
	[NeckSize] [nvarchar](max) NULL,
	[Size] [nvarchar](max) NULL,
	[GnvProductCode] [nvarchar](max) NOT NULL,
	[Supplier] [nvarchar](max) NULL,
	[IntermediateProductName] [nvarchar](max) NULL,
	[UnitPrice] [float] NOT NULL,
	[ManufactureDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[UpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Products2] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
";

    using (var uow = factory.Create()) {
      uow.Execute(new CreateTableCommand(createTableString));
      uow.Commit();
    }

    Console.WriteLine("Press any key to exit");
    Console.ReadKey();

    void PrintProduct(string productId)
    {
      using (var uow = factory.Create()) {
        var product = uow.Query(new GetProductByIdQuery(productId));

        Console.WriteLine($"Retrieved: {product.ProductName}");
      }
    }
  }

  private static async Task MainAsync(string[] args)
  {
    var factory = new UnitOfWorkFactory(ConnectionString);

    using (var uow = await factory.CreateAsync(retryOptions: new RetryOptions(5, 100, new SqlTransientExceptionDetector()))) {
      var product = await uow.QueryAsync(new GetProductByIdQuery("8304CECB-4541-4915-BD5A-3C45B8E78180"));

      Console.WriteLine($"Retrieved: {product.ProductName}");
    }
  }
}