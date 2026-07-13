using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupMemberRepository GroupMembers { get; }
        IGroupMessageReadRepository GroupMessageReads { get; }
        IPrivateMessageRepository PrivateMessages { get; }
        IUserConnectionRepository UserConnections { get; }
        IGroupRepository Groups { get; }
        IReactionRepository Reactions { get; }
        IUserBlockRepository UserBlocks { get; }
        IGroupMessageRepository GroupMessages { get; }
        IStoryRepository Stories { get; }
        IStoryViewRepository StoryViews { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();
    }
}
