using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NMT.Models;

public partial class DbNMTContext : IdentityDbContext<User>
{
    public DbSet<Result> Results { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<ShopItem> ShopItems { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<CharacterState> CharacterStates { get; set; }
    public DbSet<Achievement> Achievements { get; set; }

    public DbSet<Section> Sections { get; set; } 

    public DbNMTContext(DbContextOptions<DbNMTContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(100)");
            entity.Property(t => t.TheoryHtml).HasColumnType("nvarchar(MAX)");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(50)");
            entity.Property(e => e.Description).IsRequired().HasColumnType("nvarchar(200)");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasColumnType("nvarchar(400)");
            entity.Property(e => e.Hint).IsRequired().HasColumnType("nvarchar(200)");
            entity.Property(e => e.Difficulty).IsRequired().HasColumnType("tinyint");
            entity.Property(e => e.Image).IsRequired(false);
            entity.HasOne(e => e.QuestionType).WithMany(e => e.Questions).HasForeignKey(e => e.QuestionTypeId).HasConstraintName("FK_Questions_QuestionTypes");
            entity.HasOne(e => e.Topic).WithMany(e => e.Questions).HasForeignKey(e => e.TopicId).HasConstraintName("FK_Questions_Topics");
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OptionText).IsRequired().HasColumnType("nvarchar(100)");
            entity.Property(e => e.IsCorrect).IsRequired();
            entity.HasOne(e => e.Question).WithMany(e => e.AnswerOptions).HasForeignKey(e => e.QuestionId).HasConstraintName("FK_AnswerOptions_Questions");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Score).IsRequired();
            entity.Property(e => e.Duration).IsRequired();
            entity.HasOne(e => e.Topic).WithMany(e => e.Results).HasForeignKey(e => e.TopicId).HasConstraintName("FK_Results_Topics");
        });

        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasColumnType("nvarchar(100)");
            entity.Property(e => e.Type).IsRequired().HasColumnType("nvarchar(50)");
            entity.Property(e => e.Cost).IsRequired();
            entity.Property(e => e.ImagePath).HasColumnType("nvarchar(200)");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Inventory)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_Inventory_Users");

            entity.HasOne(e => e.ShopItem)
                .WithMany(i => i.Inventories)
                .HasForeignKey(e => e.ShopItemId)
                .HasConstraintName("FK_Inventory_ShopItems");
        });

        modelBuilder.Entity<CharacterState>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Hat).HasColumnType("nvarchar(100)");
            entity.Property(e => e.Outfit).HasColumnType("nvarchar(100)");
            entity.Property(e => e.Background).HasColumnType("nvarchar(100)");
            entity.Property(e => e.Accessory).HasColumnType("nvarchar(100)");

            entity.HasOne(e => e.User)
                .WithOne(u => u.CharacterState)
                .HasForeignKey<CharacterState>(e => e.UserId)
                .HasConstraintName("FK_CharacterState_Users");
        });

        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasColumnType("nvarchar(100)");
            entity.Property(e => e.Description).HasColumnType("nvarchar(250)");
            entity.Property(e => e.DateEarned).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Achievements)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_Achievements_Users");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

            entity.Property(e => e.Category)
                .IsRequired();

            entity.HasMany(e => e.Topics)
                .WithOne(t => t.Section)
                .HasForeignKey(t => t.SectionId)
                .HasConstraintName("FK_Topics_Sections");
        });

    }
}






