using System;
using System.Collections.Generic;
using BudgetCareApis.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetCareApis.Data;


public partial class BudgetCareDBContext : DbContext
{
	private readonly IConfiguration _config;

	public BudgetCareDBContext()
	{
	}

	public BudgetCareDBContext(DbContextOptions<BudgetCareDBContext> options, IConfiguration config)
		: base(options)
	{
		_config = config;

	}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	=> optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));

	public virtual DbSet<BondType> BondTypes { get; set; }

    public virtual DbSet<BondsDraw> BondsDraws { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DrawAnalyze> DrawAnalyzes { get; set; }

    public virtual DbSet<DrawWinsBond> DrawWinsBonds { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<NoteBook> NoteBooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BondType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.BondType1)
                .HasMaxLength(50)
                .HasColumnName("bond_type");
            entity.Property(e => e.IsPermium).HasColumnName("is_permium");
        });

        modelBuilder.Entity<BondsDraw>(entity =>
        {
            entity.HasKey(e => e.DrawId);

            entity.Property(e => e.DrawId).HasColumnName("draw_id");
            entity.Property(e => e.BondTypeId).HasColumnName("bond_type_id");
            entity.Property(e => e.Day)
                .HasMaxLength(50)
                .HasColumnName("day");
            entity.Property(e => e.DrawDate)
                .HasColumnType("datetime")
                .HasColumnName("draw_date");
            entity.Property(e => e.DrawNo).HasColumnName("draw_no");
            entity.Property(e => e.FirstPrizeWorth).HasColumnName("first_prize_worth");
            entity.Property(e => e.IsResultAnnounced).HasColumnName("is_result_announced");
            entity.Property(e => e.Place)
                .HasMaxLength(50)
                .HasColumnName("place");
            entity.Property(e => e.SecondPrizeWorth).HasColumnName("second_prize_worth");
            entity.Property(e => e.ThirdPrizeWorth).HasColumnName("third_prize_worth");

            entity.HasOne(d => d.BondType).WithMany(p => p.BondsDraws)
                .HasForeignKey(d => d.BondTypeId)
                .HasConstraintName("FK_BondsDraws_BondTypes");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F5E9A4269");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(3500)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Title)
                .HasMaxLength(1000)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Categories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Category__userId__4D5F7D71");
        });

        modelBuilder.Entity<DrawAnalyze>(entity =>
        {
            entity.ToTable("DrawAnalyze");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Json).HasColumnName("json");
        });

        modelBuilder.Entity<DrawWinsBond>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BoundNo)
                .HasMaxLength(50)
                .HasColumnName("bound_no");
            entity.Property(e => e.DrawId).HasColumnName("draw_id");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.HasOne(d => d.Draw).WithMany(p => p.DrawWinsBonds)
                .HasForeignKey(d => d.DrawId)
                .HasConstraintName("FK_DrawWinsBonds_BondsDraws");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expense__3213E83F74EAE88D");

            entity.ToTable("Expense");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnName("description");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Income__3213E83F3D73790A");

            entity.ToTable("Income");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Desciption)
                .HasMaxLength(2000)
                .HasColumnName("desciption");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.NoteBookId).HasColumnName("note_book_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.NoteBook).WithMany(p => p.Notes)
                .HasForeignKey(d => d.NoteBookId)
                .HasConstraintName("FK_Notes_NoteBook");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Notes_User");
        });

        modelBuilder.Entity<NoteBook>(entity =>
        {
            entity.ToTable("NoteBook");

            entity.Property(e => e.NoteBookId).HasColumnName("note_book_id");
            entity.Property(e => e.IconColor)
                .HasMaxLength(50)
                .HasColumnName("icon_color");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.NoteBooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoteBook_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F99B72138");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.Image)
                .HasMaxLength(4000)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(3000)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
