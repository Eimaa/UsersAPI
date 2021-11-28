using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using UsersAPI.Models;

namespace UsersAPI.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}