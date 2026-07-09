using RealTimeChatApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.Repositories
{
    public interface IPrivateMessageRepository : IGenericRepository<PrivateMessage>
    {
    }
}
