using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class FileRepo(AppDbContext context) : IFileRepo
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(FileEntity fileE)
            => await _context.AddAsync(fileE);

        public async Task<FileEntity?> GetById(Guid id, Guid UserId)
            => await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);
    }
}
