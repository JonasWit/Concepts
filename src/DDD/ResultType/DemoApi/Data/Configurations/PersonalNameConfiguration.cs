using DemoApi.Data.Common;
using DemoApi.Data.Converters;
using DemoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApi.Data.Configurations;

class PersonalNameConfiguration : IComplexPropertyConfiguration<PersonalName>
{
    public ComplexPropertyBuilder<PersonalName> Configure(ComplexPropertyBuilder<PersonalName> builder)
    {
        builder.Property(name => name.First).HasColumnName("FirstName");

        builder.Property(name => name.Middle).HasColumnName("MiddleNames")
            .HasConversion(new NullableStringConverter())
            .IsRequired(false);
        
        builder.Property(name => name.Last).HasColumnName("LastName");

        return builder;
    }
}
