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
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder
    .HasIndex(x => new
    {
        x.PrivateMessageId,
        x.UserId
    })
    .IsUnique()
    .HasFilter("[PrivateMessageId] IS NOT NULL");
            builder
    .HasIndex(x => new
    {
        x.GroupMessageId,
        x.UserId
    })
    .IsUnique()
    .HasFilter("[GroupMessageId] IS NOT NULL");
        }
    }
}
