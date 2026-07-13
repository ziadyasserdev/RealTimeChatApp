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
    public class StoryViewConfiguration
     : IEntityTypeConfiguration<StoryView>
    {
        public void Configure(EntityTypeBuilder<StoryView> builder)
        {

            builder.HasKey(x => x.Id);



            builder.HasOne(x => x.Story)
                .WithMany(x => x.Views)
                .HasForeignKey(x => x.StoryId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(x => x.Viewer)
                .WithMany()
                .HasForeignKey(x => x.ViewerId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
