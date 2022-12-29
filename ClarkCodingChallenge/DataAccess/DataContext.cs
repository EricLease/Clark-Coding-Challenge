using Microsoft.EntityFrameworkCore;
using ClarkCodingChallenge.DataAccess.Models;

namespace ClarkCodingChallenge.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Contact> Contacts { get; set; }
    }
}
