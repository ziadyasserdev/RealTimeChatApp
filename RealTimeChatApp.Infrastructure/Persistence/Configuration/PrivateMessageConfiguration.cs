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
    public class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
    {
        public void Configure(EntityTypeBuilder<PrivateMessage> builder)
        {
            builder.ToTable("PrivateMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                   .HasMaxLength(4000);

            builder.Property(x => x.MessageType)
                   .HasConversion<string>();

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.SentPrivateMessages)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Receiver)
                   .WithMany(x => x.ReceivedPrivateMessages)
                   .HasForeignKey(x => x.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
