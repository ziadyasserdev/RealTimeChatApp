using Microsoft.EntityFrameworkCore.Storage;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext _context)
        {
            this._context = _context;
            Reactions = new ReactionRepository(_context);
            UserBlocks = new UserBlockRepository(_context);
            GroupMembers = new GroupMemberRepository(_context);
            GroupMessageReads = new GroupMessageReadRepository(_context);
            PrivateMessages = new PrivateMessageRepository(_context);
            UserConnections = new UserConnectionRepository(_context);
            Groups = new GroupRepository(_context);
            GroupMessages = new GroupMessageRepository(_context);
        }
        public IGroupMemberRepository GroupMembers { get; private set; }

        public IGroupMessageReadRepository GroupMessageReads { get; private set; }

        public IPrivateMessageRepository PrivateMessages { get; private set; }

        public IUserConnectionRepository UserConnections { get; private set; }

        public IGroupRepository Groups { get; private set; }

        public IGroupMessageRepository GroupMessages { get; private set; }

        public IReactionRepository Reactions { get; private set; }

        public IUserBlockRepository UserBlocks {  get; private set; }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
