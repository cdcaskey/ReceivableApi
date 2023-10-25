using Microsoft.EntityFrameworkCore;
using ReceivableApi.Models;

namespace ReceivableApi.Data
{
    public class ReceivableApiContext : DbContext
    {
        public ReceivableApiContext(DbContextOptions<ReceivableApiContext> options)
            : base(options)
        {
        }

        public DbSet<Debtor> Debtors { get; set; } = default!;

        public DbSet<Receivable> Receivables { get; set; } = default!;
    }
}
