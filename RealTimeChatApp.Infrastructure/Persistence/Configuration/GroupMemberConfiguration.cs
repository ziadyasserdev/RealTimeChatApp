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
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.ToTable("GroupMembers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Role)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(x => x.Nickname)
                   .HasMaxLength(50);

            builder.HasIndex(x => new { x.GroupId, x.UserId })
                   .IsUnique();

            builder.HasOne(x => x.Group)
                   .WithMany(x => x.Members)
                   .HasForeignKey(x => x.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.GroupMemberships)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
          
        }
    }
}
