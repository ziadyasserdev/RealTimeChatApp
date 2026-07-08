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
    public class GroupMessageReadConfiguration : IEntityTypeConfiguration<GroupMessageRead>
    {
        public void Configure(EntityTypeBuilder<GroupMessageRead> builder)
        {
            builder.ToTable("GroupMessageReads");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.GroupMessageId, x.UserId })
                   .IsUnique();

            builder.HasOne(x => x.GroupMessage)
                   .WithMany(x => x.Reads)
                   .HasForeignKey(x => x.GroupMessageId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
