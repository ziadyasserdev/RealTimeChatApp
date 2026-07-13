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
    public class UserBlockConfiguration : IEntityTypeConfiguration<UserBlock>
    {
        public void Configure(EntityTypeBuilder<UserBlock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Blocker)
                .WithMany(x => x.BlockedUsers)
                .HasForeignKey(x => x.BlockerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.BlockedUser)
                .WithMany(x => x.BlockedByUsers)
                .HasForeignKey(x => x.BlockedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new
            {
                x.BlockerId,
                x.BlockedUserId
            }).IsUnique();
        }
    }
}
