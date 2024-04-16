using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoGPLX.Models;

public partial class DbGplxContext : DbContext
{
    public DbGplxContext()
    {
    }

    public DbGplxContext(DbContextOptions<DbGplxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cau> Caus { get; set; }

    public virtual DbSet<Chuong> Chuongs { get; set; }

    public virtual DbSet<Dapan> Dapans { get; set; }

    public virtual DbSet<De> Des { get; set; }

    public virtual DbSet<Hang> Hangs { get; set; }

    public virtual DbSet<HangCau> HangCaus { get; set; }

    public virtual DbSet<HangChuong> HangChuongs { get; set; }

    public virtual DbSet<Ttcau> Ttcaus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=db-gplx;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cau>(entity =>
        {
            entity.HasKey(e => e.IdCau).HasName("PRIMARY");

            entity.ToTable("cau");

            entity.HasIndex(e => e.IdChuong, "id_chuong");

            entity.Property(e => e.IdCau)
                .ValueGeneratedNever()
                .HasColumnName("id_cau");
            entity.Property(e => e.IdChuong).HasColumnName("id_chuong");
            entity.Property(e => e.Stt).HasColumnName("stt");

            entity.HasOne(d => d.IdChuongNavigation).WithMany(p => p.Caus)
                .HasForeignKey(d => d.IdChuong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cau_ibfk_1");
        });

        modelBuilder.Entity<Chuong>(entity =>
        {
            entity.HasKey(e => e.IdChuong).HasName("PRIMARY");

            entity.ToTable("chuong");

            entity.Property(e => e.IdChuong)
                .ValueGeneratedNever()
                .HasColumnName("id_chuong");
            entity.Property(e => e.ThongTinChuong)
                .HasColumnType("text")
                .HasColumnName("thong_tin_chuong");
        });

        modelBuilder.Entity<Dapan>(entity =>
        {
            entity.HasKey(e => new { e.IdDapan, e.IdCau })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("dapan");

            entity.HasIndex(e => e.IdCau, "id_cau");

            entity.Property(e => e.IdDapan).HasColumnName("id_dapan");
            entity.Property(e => e.IdCau).HasColumnName("id_cau");
            entity.Property(e => e.Dapan1)
                .HasColumnType("text")
                .HasColumnName("dapan");
            entity.Property(e => e.Dapandung).HasColumnName("dapandung");

            entity.HasOne(d => d.IdCauNavigation).WithMany(p => p.Dapans)
                .HasForeignKey(d => d.IdCau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dapan_ibfk_1");
        });

        modelBuilder.Entity<De>(entity =>
        {
            entity.HasKey(e => e.IdDe).HasName("PRIMARY");

            entity.ToTable("de");

            entity.HasIndex(e => e.IdHang, "id_hang");

            entity.Property(e => e.IdDe)
                .ValueGeneratedNever()
                .HasColumnName("id_de");
            entity.Property(e => e.IdHang).HasColumnName("id_hang");

            entity.HasOne(d => d.IdHangNavigation).WithMany(p => p.Des)
                .HasForeignKey(d => d.IdHang)
                .HasConstraintName("de_ibfk_1");

            entity.HasMany(d => d.IdCaus).WithMany(p => p.IdDes)
                .UsingEntity<Dictionary<string, object>>(
                    "DeCau",
                    r => r.HasOne<Cau>().WithMany()
                        .HasForeignKey("IdCau")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("de_cau_ibfk_2"),
                    l => l.HasOne<De>().WithMany()
                        .HasForeignKey("IdDe")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("de_cau_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdDe", "IdCau")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("de_cau");
                        j.HasIndex(new[] { "IdCau" }, "id_cau");
                        j.IndexerProperty<int>("IdDe").HasColumnName("id_de");
                        j.IndexerProperty<int>("IdCau").HasColumnName("id_cau");
                    });
        });

        modelBuilder.Entity<Hang>(entity =>
        {
            entity.HasKey(e => e.IdHang).HasName("PRIMARY");

            entity.ToTable("hang");

            entity.Property(e => e.IdHang)
                .ValueGeneratedNever()
                .HasColumnName("id_hang");
            entity.Property(e => e.Diemtoida).HasColumnName("diemtoida");
            entity.Property(e => e.Diemtoitheu).HasColumnName("diemtoitheu");
            entity.Property(e => e.Thoigianthi).HasColumnName("thoigianthi");
            entity.Property(e => e.Thongtin)
                .HasMaxLength(3)
                .HasColumnName("thongtin");
            entity.Property(e => e.Thongtinchitiet)
                .HasColumnType("text")
                .HasColumnName("thongtinchitiet");
        });

        modelBuilder.Entity<HangCau>(entity =>
        {
            entity.HasKey(e => new { e.IdHang, e.IdCau })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("hang_cau");

            entity.HasIndex(e => e.IdCau, "id_cau");

            entity.Property(e => e.IdHang).HasColumnName("id_hang");
            entity.Property(e => e.IdCau).HasColumnName("id_cau");
            entity.Property(e => e.Index).HasColumnName("index");

            entity.HasOne(d => d.IdCauNavigation).WithMany(p => p.HangCaus)
                .HasForeignKey(d => d.IdCau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_cau_ibfk_2");

            entity.HasOne(d => d.IdHangNavigation).WithMany(p => p.HangCaus)
                .HasForeignKey(d => d.IdHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_cau_ibfk_1");
        });

        modelBuilder.Entity<HangChuong>(entity =>
        {
            entity.HasKey(e => new { e.IdHang, e.IdChuong })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("hang_chuong");

            entity.HasIndex(e => e.IdChuong, "id_chuong");

            entity.Property(e => e.IdHang).HasColumnName("id_hang");
            entity.Property(e => e.IdChuong).HasColumnName("id_chuong");
            entity.Property(e => e.Soluong).HasColumnName("soluong");

            entity.HasOne(d => d.IdChuongNavigation).WithMany(p => p.HangChuongs)
                .HasForeignKey(d => d.IdChuong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_chuong_ibfk_2");

            entity.HasOne(d => d.IdHangNavigation).WithMany(p => p.HangChuongs)
                .HasForeignKey(d => d.IdHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hang_chuong_ibfk_1");
        });

        modelBuilder.Entity<Ttcau>(entity =>
        {
            entity.HasKey(e => e.IdTtcau).HasName("PRIMARY");

            entity.ToTable("ttcau");

            entity.HasIndex(e => e.IdCau, "id_cau");

            entity.Property(e => e.IdTtcau)
                .ValueGeneratedNever()
                .HasColumnName("id_ttcau");
            entity.Property(e => e.Cauhoi)
                .HasColumnType("text")
                .HasColumnName("cauhoi");
            entity.Property(e => e.Diemliet).HasColumnName("diemliet");
            entity.Property(e => e.Goiy)
                .HasColumnType("text")
                .HasColumnName("goiy");
            entity.Property(e => e.Hinhcauhoi)
                .HasColumnType("mediumblob")
                .HasColumnName("hinhcauhoi");
            entity.Property(e => e.IdCau).HasColumnName("id_cau");

            entity.HasOne(d => d.IdCauNavigation).WithMany(p => p.Ttcaus)
                .HasForeignKey(d => d.IdCau)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ttcau_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
