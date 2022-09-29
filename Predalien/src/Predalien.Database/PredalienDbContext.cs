namespace Predalien.Database;

using Microsoft.EntityFrameworkCore;

public class Article
{
    public Guid ArticleId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
}

public class Comment
{
    public Guid CommentId { get; set; }
    public string Text { get; set; }
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
}

public class PredalienDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PredalienDb");
    }
}