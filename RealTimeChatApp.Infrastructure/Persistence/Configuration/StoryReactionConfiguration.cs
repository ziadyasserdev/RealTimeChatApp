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
    public class StoryReactionConfiguration
    : IEntityTypeConfiguration<StoryReaction>
    {
        public void Configure(EntityTypeBuilder<StoryReaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ReactionType)
                .IsRequired();

            builder.Property(x => x.ReactedAt)
                .IsRequired();

            
            builder.HasIndex(x => new
            {
                x.StoryId,
                x.UserId
            })
            .IsUnique();

            builder.HasOne(x => x.Story)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
