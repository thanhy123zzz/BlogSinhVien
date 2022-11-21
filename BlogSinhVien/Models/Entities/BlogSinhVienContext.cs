using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class BlogSinhVienContext : DbContext
    {
        public BlogSinhVienContext()
        {
        }

        public BlogSinhVienContext(DbContextOptions<BlogSinhVienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<BaiDang> BaiDang { get; set; }
        public virtual DbSet<BinhLuan> BinhLuan { get; set; }
        public virtual DbSet<ChiTietBaiDang> ChiTietBaiDang { get; set; }
        public virtual DbSet<ChiTietCmt> ChiTietCmt { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<QuanLy> QuanLy { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-FPKNLS6A\\SQLEXPRESS;Initial Catalog=BlogSinhVien;Persist Security Info=True;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan)
                    .HasName("PK__Accounts__D5B8C7F1F7F7051E");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaRole)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.MaRoleNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.MaRole)
                    .HasConstraintName("FK__Accounts__MaRole__47DBAE45");
            });

            modelBuilder.Entity<BaiDang>(entity =>
            {
                entity.HasKey(e => e.MaBaiDang)
                    .HasName("PK__BaiDang__BF5D50C575E01ADA");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.MaQl)
                    .HasColumnName("MaQL")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSinhVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.HasOne(d => d.MaQlNavigation)
                    .WithMany(p => p.BaiDang)
                    .HasForeignKey(d => d.MaQl)
                    .HasConstraintName("FK__BaiDang__MaQL__48CFD27E");

                entity.HasOne(d => d.MaSinhVienNavigation)
                    .WithMany(p => p.BaiDang)
                    .HasForeignKey(d => d.MaSinhVien)
                    .HasConstraintName("FK__BaiDang__MaSinhV__49C3F6B7");
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.HasKey(e => e.MaCmt)
                    .HasName("PK__BinhLuan__3DCB2FA0ADBCF06D");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.MaQl)
                    .HasColumnName("MaQL")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSinhVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.HasOne(d => d.MaBaiDangNavigation)
                    .WithMany(p => p.BinhLuan)
                    .HasForeignKey(d => d.MaBaiDang)
                    .HasConstraintName("FK__BinhLuan__MaBaiD__4AB81AF0");

                entity.HasOne(d => d.MaQlNavigation)
                    .WithMany(p => p.BinhLuan)
                    .HasForeignKey(d => d.MaQl)
                    .HasConstraintName("FK__BinhLuan__MaQL__4BAC3F29");

                entity.HasOne(d => d.MaSinhVienNavigation)
                    .WithMany(p => p.BinhLuan)
                    .HasForeignKey(d => d.MaSinhVien)
                    .HasConstraintName("FK__BinhLuan__MaSinh__4CA06362");
            });

            modelBuilder.Entity<ChiTietBaiDang>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Files).HasColumnName("files");

                entity.Property(e => e.NameFile)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.MaBaiDangNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaBaiDang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietBa__MaBai__2BFE89A6");
            });

            modelBuilder.Entity<ChiTietCmt>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Files).HasColumnName("files");

                entity.Property(e => e.NameFile)
                    .HasColumnName("nameFile")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.MaCmtNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaCmt)
                    .HasConstraintName("FK__ChiTietCm__MaCmt__4E88ABD4");
            });

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.MaC)
                    .HasName("PK__Conversa__C7977BB5E9651322");

                entity.Property(e => e.MaSinhVien1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSinhVien2)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.MaSinhVien1Navigation)
                    .WithMany(p => p.ConversationMaSinhVien1Navigation)
                    .HasForeignKey(d => d.MaSinhVien1)
                    .HasConstraintName("FK__Conversat__MaSin__5070F446");

                entity.HasOne(d => d.MaSinhVien2Navigation)
                    .WithMany(p => p.ConversationMaSinhVien2Navigation)
                    .HasForeignKey(d => d.MaSinhVien2)
                    .HasConstraintName("FK__Conversat__MaSin__5165187F");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.MaSinhVienReceive)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSinhVienSend)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.HasOne(d => d.MaCNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaC)
                    .HasConstraintName("FK__Message__MaC__60A75C0F");

                entity.HasOne(d => d.MaSinhVienReceiveNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaSinhVienReceive)
                    .HasConstraintName("FK__Message__MaSinhV__628FA481");

                entity.HasOne(d => d.MaSinhVienSendNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaSinhVienSend)
                    .HasConstraintName("FK__Message__MaSinhV__619B8048");
            });

            modelBuilder.Entity<QuanLy>(entity =>
            {
                entity.HasKey(e => e.MaQl)
                    .HasName("PK__QuanLy__2725F8529B9216E7");

                entity.Property(e => e.MaQl)
                    .HasColumnName("MaQL")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cccd)
                    .HasColumnName("CCCD")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.EmailEdu)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.EmailPrivate)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Ho).HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ten).HasMaxLength(20);

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.QuanLy)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK__QuanLy__TaiKhoan__6383C8BA");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.MaRole)
                    .HasName("PK__Roles__0639A0FD64A7BCB3");

                entity.Property(e => e.MaRole)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TenRole).HasMaxLength(50);
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.MaSv)
                    .HasName("PK__SinhVien__2725081AF8715473");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cccd)
                    .HasColumnName("CCCD")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.EmailEdu)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.EmailPrivate)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Ho).HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ten).HasMaxLength(20);

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.SinhVien)
                    .HasForeignKey(d => d.TaiKhoan)
                    .HasConstraintName("FK__SinhVien__TaiKho__656C112C");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => new { e.MaCmt, e.MaUser });

                entity.Property(e => e.MaUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TimeVote).HasColumnType("datetime");

                entity.HasOne(d => d.MaCmtNavigation)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.MaCmt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vote__MaCmt__3C34F16F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
