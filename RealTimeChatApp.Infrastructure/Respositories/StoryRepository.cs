using RealTimeChatApp.Application.Contracts.Repositories;
using RealTimeChatApp.Domain.Models;
using RealTimeChatApp.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Respositories
{
    public class StoryRepository : GenericRepository<Story>, IStoryRepository
    {
        public StoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
