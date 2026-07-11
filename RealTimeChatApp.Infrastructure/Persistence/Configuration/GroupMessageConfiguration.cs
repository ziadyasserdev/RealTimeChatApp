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
    public class GroupMessageConfiguration : IEntityTypeConfiguration<GroupMessage>
    {
        public void Configure(EntityTypeBuilder<GroupMessage> builder)
        {
            builder.ToTable("GroupMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                   .HasMaxLength(4000);

            builder.Property(x => x.MessageType)
                   .HasConversion<string>();

            builder.HasOne(x => x.Group)
                   .WithMany(x => x.Messages)
                   .HasForeignKey(x => x.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.GroupMessages)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.ReplyToMessage)
    .WithMany()
    .HasForeignKey(x => x.ReplyToMessageId)
    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
