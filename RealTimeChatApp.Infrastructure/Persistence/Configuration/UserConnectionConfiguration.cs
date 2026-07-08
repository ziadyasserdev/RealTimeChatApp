using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Persistence.Configuration
{
    public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.ToTable("UserConnections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ConnectionId)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.HasIndex(x => x.ConnectionId)
                   .IsUnique();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Connections)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
