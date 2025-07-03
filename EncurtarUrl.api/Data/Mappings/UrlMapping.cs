using EncurtarUrl.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EncurtarUrl.api.Data.Mappings
{
    public class UrlMapping : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.ToTable("Urls");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.OriginalUrl)
            .IsRequired(true)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(2048);

            builder.Property(u => u.ShortenedCode)
            .IsRequired(true)
            .HasColumnType("VARCHAR")
            .HasMaxLength(10);

            builder.Property(u => u.ShortenedUrl)
            .IsRequired(true)
            .HasColumnType("VARCHAR")
            .HasMaxLength(100);

            builder.Property(u => u.ClickCount)
            .IsRequired(true)
            .HasColumnType("INT")
            .HasDefaultValue(0);

            builder.Property(u => u.CreatedAt)
            .IsRequired(true)
            .HasDefaultValueSql("GETDATE()");

            // builder.Property(u => u.ExpirationDate)
            // .IsRequired(false);


        }
    }
}