using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Common;

internal interface IComplexPropertyConfiguration<TEntity> where TEntity : class
{
    ComplexPropertyBuilder<TEntity> Configure(ComplexPropertyBuilder<TEntity> builder);
}