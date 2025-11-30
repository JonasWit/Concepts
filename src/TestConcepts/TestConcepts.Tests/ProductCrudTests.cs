using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TestConcepts.Tests;

public class ProductCrudTests
{
    [Fact]
    public async Task AddReadAndRemoveProduct_WithMockedDbSet()
    {
        var products = new List<Product>();
        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var mockContext = Substitute.For<AppDbContext>(
            new DbContextOptionsBuilder<AppDbContext>().Options);
        mockContext.Products.Returns(mockSet);
        mockContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var newProduct = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 99.99m,
            IsActive = true
        };

        mockContext.Products.Add(newProduct);
        await mockContext.SaveChangesAsync();

        Assert.Single(products);
        Assert.Equal("Test Product", products[0].Name);

        var retrievedProduct = mockContext.Products.FirstOrDefault(p => p.Id == 1);

        Assert.NotNull(retrievedProduct);
        Assert.Equal(1, retrievedProduct.Id);
        Assert.Equal("Test Product", retrievedProduct.Name);
        Assert.Equal(99.99m, retrievedProduct.Price);

        mockContext.Products.Remove(retrievedProduct);
        await mockContext.SaveChangesAsync();

        Assert.Empty(products);

        var deletedProduct = mockContext.Products.FirstOrDefault(p => p.Id == 1);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task GetActiveProductsAsync_ShouldReturnOnlyActiveProducts()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);

        context.Products.AddRange(
            new Product { Id = 1, Name = "Active Product 1", Price = 10.99m, IsActive = true },
            new Product { Id = 2, Name = "Inactive Product", Price = 20.99m, IsActive = false },
            new Product { Id = 3, Name = "Active Product 2", Price = 15.99m, IsActive = true }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetActiveProductsAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.True(p.IsActive));
        Assert.Equal("Active Product 1", result[0].Name);
        Assert.Equal("Active Product 2", result[1].Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnCorrectProduct()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);

        var testProduct = new Product { Id = 1, Name = "Test Product", Price = 25.50m, IsActive = true };
        context.Products.Add(testProduct);
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal(25.50m, result.Price);
    }

    [Fact]
    public async Task GetProductCountAsync_ShouldReturnCorrectCount()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        using var context = new AppDbContext(options);

        context.Products.AddRange(
            new Product { Id = 1, Name = "Product 1", Price = 10m, IsActive = true },
            new Product { Id = 2, Name = "Product 2", Price = 20m, IsActive = false },
            new Product { Id = 3, Name = "Product 3", Price = 30m, IsActive = true }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var count = await service.GetProductCountAsync();

        Assert.Equal(3, count);
    }

    [Fact]
    public void MockDbContext_WithNSubstitute_Example()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Mocked Product", Price = 99.99m, IsActive = true }
        };

        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var mockContext = Substitute.For<AppDbContext>(
            new DbContextOptionsBuilder<AppDbContext>().Options);
        mockContext.Products.Returns(mockSet);

        var productsList = mockContext.Products.ToList();
        Assert.Single(productsList);
        Assert.Equal("Mocked Product", productsList[0].Name);
    }

    [Fact]
    public async Task InMemoryDatabase_Example()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        await using var context = new AppDbContext(options);

        context.Products.Add(new Product
        {
            Id = 1,
            Name = "In-Memory Product",
            Price = 49.99m,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == 1);

        Assert.NotNull(product);
        Assert.Equal("In-Memory Product", product.Name);
    }

    [Fact]
    public void MockDbSet_WithQueryableData()
    {
        var data = new List<Product>
        {
            new() { Id = 1, Name = "Product A", Price = 10m, IsActive = true },
            new() { Id = 2, Name = "Product B", Price = 20m, IsActive = false },
            new() { Id = 3, Name = "Product C", Price = 30m, IsActive = true }
        };

        var mockSet = DbSetMockHelper.CreateMockDbSet(data);

        var mockContext = Substitute.For<AppDbContext>(
            new DbContextOptionsBuilder<AppDbContext>().Options);
        mockContext.Products.Returns(mockSet);

        var activeProducts = mockContext.Products
            .Where(p => p.IsActive)
            .ToList();

        Assert.Equal(2, activeProducts.Count);
        Assert.All(activeProducts, p => Assert.True(p.IsActive));
    }

    [Fact]
    public async Task MockDbSet_FindAsync()
    {
        var expectedProduct = new Product
        {
            Id = 1,
            Name = "Found Product",
            Price = 100m,
            IsActive = true
        };

        var mockSet = Substitute.For<DbSet<Product>>();
        mockSet.FindAsync(1).Returns(expectedProduct);

        var mockContext = Substitute.For<AppDbContext>(
            new DbContextOptionsBuilder<AppDbContext>().Options);
        mockContext.Products.Returns(mockSet);

        var service = new ProductService(mockContext);

        var result = await service.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Found Product", result.Name);
        await mockSet.Received(1).FindAsync(1);
    }

    [Fact]
    public async Task InMemoryDatabase_EmptyDataset()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"EmptyDb_{Guid.NewGuid()}")
            .Options;

        await using var context = new AppDbContext(options);
        var service = new ProductService(context);

        var products = await service.GetActiveProductsAsync();
        var count = await service.GetProductCountAsync();

        Assert.Empty(products);
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task VerifyContextMethodCalls()
    {
        var products = new List<Product>();
        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var mockContext = Substitute.For<AppDbContext>(
            new DbContextOptionsBuilder<AppDbContext>().Options);
        mockContext.Products.Returns(mockSet);

        mockContext.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        mockContext.Products.Add(new Product
        {
            Id = 1,
            Name = "New Product",
            Price = 50m,
            IsActive = true
        });
        await mockContext.SaveChangesAsync();

        mockSet.Received(1).Add(Arg.Is<Product>(p => p.Name == "New Product"));
        await mockContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void MockDbSet_AddRange_ShouldAddMultipleProducts()
    {
        var products = new List<Product>();
        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var newProducts = new[]
        {
            new Product { Id = 1, Name = "Product 1", Price = 10m, IsActive = true },
            new Product { Id = 2, Name = "Product 2", Price = 20m, IsActive = false }
        };

        mockSet.AddRange(newProducts);

        Assert.Equal(2, products.Count);
        Assert.Contains(products, p => p.Name == "Product 1");
        Assert.Contains(products, p => p.Name == "Product 2");
    }

    [Fact]
    public void MockDbSet_Update_ShouldModifyProduct()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Original Name", Price = 10m, IsActive = true }
        };
        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var productToUpdate = products[0];
        productToUpdate.Name = "Updated Name";
        productToUpdate.Price = 20m;

        mockSet.Update(productToUpdate);

        Assert.Equal("Updated Name", products[0].Name);
        Assert.Equal(20m, products[0].Price);
    }

    [Fact]
    public void MockDbSet_RemoveRange_ShouldRemoveMultipleProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Product 1", Price = 10m, IsActive = true },
            new() { Id = 2, Name = "Product 2", Price = 20m, IsActive = false },
            new() { Id = 3, Name = "Product 3", Price = 30m, IsActive = true }
        };
        var mockSet = DbSetMockHelper.CreateMockDbSet(products);

        var productsToRemove = products.Where(p => p.IsActive).ToList();
        mockSet.RemoveRange(productsToRemove);

        Assert.Single(products);
        Assert.Equal("Product 2", products[0].Name);
    }
}
