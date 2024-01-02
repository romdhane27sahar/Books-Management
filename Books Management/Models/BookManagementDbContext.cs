using Microsoft.EntityFrameworkCore;

namespace Books_Management.Models
{
    public class BookManagementDbContext :DbContext
    {
        public BookManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        //DBSET des livres
        public DbSet<Book> Books { get; set; } = null!;
        //DBSET des auteurs 
        public DbSet<Author> Authors { get; set; } = null!;

    }

}

