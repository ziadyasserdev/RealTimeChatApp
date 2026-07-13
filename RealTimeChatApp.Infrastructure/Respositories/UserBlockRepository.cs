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
    public class UserBlockRepository : GenericRepository<UserBlock>, IUserBlockRepository
    {
        public UserBlockRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
