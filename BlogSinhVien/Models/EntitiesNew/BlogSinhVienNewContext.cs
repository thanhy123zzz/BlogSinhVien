using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class BlogSinhVienNewContext : DbContext
    {
        public BlogSinhVienNewContext()
        {
        }

        public BlogSinhVienNewContext(DbContextOptions<BlogSinhVienNewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<BaiDang> BaiDang { get; set; }
        public virtual DbSet<BinhLuan> BinhLuan { get; set; }
        public virtual DbSet<ChiTietBaiDang> ChiTietBaiDang { get; set; }
        public virtual DbSet<ChiTietBinhLuan> ChiTietBinhLuan { get; set; }
        public virtual DbSet<ChucNang> ChucNang { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<ToCao> ToCao { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VaiTro> VaiTro { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-FPKNLS6A\\SQLEXPRESS;Initial Catalog=BlogSinhVienNew;Persist Security Info=True;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasIndex(e => e.TaiKhoan)
                    .HasName("UQ__Accounts__D5B8C7F0F612B67F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdvaiTro).HasColumnName("IDVaiTro");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.MatKhau).HasMaxLength(50);

                entity.Property(e => e.TaiKhoan)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdvaiTroNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.IdvaiTro)
                    .HasConstraintName("FK__Accounts__IDVaiT__5FB337D6");
            });

            modelBuilder.Entity<BaiDang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).HasMaxLength(1000);

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.Property(e => e.NgayGhim).HasColumnType("datetime");

                entity.Property(e => e.Slreport).HasColumnName("SLReport");

                entity.HasOne(d => d.IdUserGhimNavigation)
                    .WithMany(p => p.BaiDangIdUserGhimNavigation)
                    .HasForeignKey(d => d.IdUserGhim)
                    .HasConstraintName("FK__BaiDang__IdUserG__17036CC0");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.BaiDangIduserNavigation)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK__BaiDang__IDUser__6754599E");
            });

            modelBuilder.Entity<BinhLuan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).HasMaxLength(1000);

                entity.Property(e => e.IdbaiDang).HasColumnName("IDBaiDang");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.NgayDang).HasColumnType("datetime");

                entity.Property(e => e.Sllike).HasColumnName("SLLike");

                entity.HasOne(d => d.IdbaiDangNavigation)
                    .WithMany(p => p.BinhLuan)
                    .HasForeignKey(d => d.IdbaiDang)
                    .HasConstraintName("FK__BinhLuan__IDBaiD__6FE99F9F");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.BinhLuan)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK__BinhLuan__IDUser__70DDC3D8");
            });

            modelBuilder.Entity<ChiTietBaiDang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdbaiDang).HasColumnName("IDBaiDang");

                entity.Property(e => e.NameFile).HasMaxLength(200);

                entity.Property(e => e.Path).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(200);

                entity.HasOne(d => d.IdbaiDangNavigation)
                    .WithMany(p => p.ChiTietBaiDang)
                    .HasForeignKey(d => d.IdbaiDang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietBa__IDBai__6A30C649");
            });

            modelBuilder.Entity<ChiTietBinhLuan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdbinhLuan).HasColumnName("IDBinhLuan");

                entity.Property(e => e.NameFile)
                    .HasColumnName("nameFile")
                    .HasMaxLength(200);

                entity.Property(e => e.Path).HasMaxLength(200);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(200);

                entity.HasOne(d => d.IdbinhLuanNavigation)
                    .WithMany(p => p.ChiTietBinhLuan)
                    .HasForeignKey(d => d.IdbinhLuan)
                    .HasConstraintName("FK__ChiTietBi__IDBin__73BA3083");
            });

            modelBuilder.Entity<ChucNang>(entity =>
            {
                entity.HasIndex(e => e.MaChucNang)
                    .HasName("UQ__ChucNang__B26DC256ABC9460D")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdvaiTro).HasColumnName("IDVaiTro");

                entity.Property(e => e.MaChucNang).HasMaxLength(50);

                entity.Property(e => e.TenChucNang).HasMaxLength(50);

                entity.HasOne(d => d.IdvaiTroNavigation)
                    .WithMany(p => p.ChucNang)
                    .HasForeignKey(d => d.IdvaiTro)
                    .HasConstraintName("FK__ChucNang__IDVaiT__5AEE82B9");
            });

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Iduser1).HasColumnName("IDUser1");

                entity.Property(e => e.Iduser2).HasColumnName("IDUser2");

                entity.Property(e => e.IduserLast).HasColumnName("IDUserLast");

                entity.Property(e => e.LastTime).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.HasOne(d => d.Iduser1Navigation)
                    .WithMany(p => p.ConversationIduser1Navigation)
                    .HasForeignKey(d => d.Iduser1)
                    .HasConstraintName("FK__Conversat__IDUse__76969D2E");

                entity.HasOne(d => d.Iduser2Navigation)
                    .WithMany(p => p.ConversationIduser2Navigation)
                    .HasForeignKey(d => d.Iduser2)
                    .HasConstraintName("FK__Conversat__IDUse__778AC167");

                entity.HasOne(d => d.IduserLastNavigation)
                    .WithMany(p => p.ConversationIduserLastNavigation)
                    .HasForeignKey(d => d.IduserLast)
                    .HasConstraintName("FK__Conversat__IDUse__0D7A0286");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.Idc).HasColumnName("IDC");

                entity.Property(e => e.IduserSend).HasColumnName("IDUserSend");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.HasOne(d => d.IdcNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.Idc)
                    .HasConstraintName("FK__Message__IDC__04E4BC85");

                entity.HasOne(d => d.IduserSendNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.IduserSend)
                    .HasConstraintName("FK__Message__IDUserS__7E37BEF6");
            });

            modelBuilder.Entity<ToCao>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idbd).HasColumnName("IDBD");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.NgayToCao).HasColumnType("datetime");

                entity.HasOne(d => d.IdbdNavigation)
                    .WithMany(p => p.ToCao)
                    .HasForeignKey(d => d.Idbd)
                    .HasConstraintName("FK__ToCao__IDBD__1332DBDC");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.ToCao)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK__ToCao__IDUser__123EB7A3");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.MaSv)
                    .HasName("UQ__Users__2725081B6A34D68E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cccd)
                    .HasColumnName("CCCD")
                    .HasMaxLength(15);

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.EmailEdu).HasMaxLength(80);

                entity.Property(e => e.EmailPrivate).HasMaxLength(80);

                entity.Property(e => e.GhiChu).HasMaxLength(2000);

                entity.Property(e => e.HinhAnh).HasMaxLength(100);

                entity.Property(e => e.Ho).HasMaxLength(50);

                entity.Property(e => e.Idtk).HasColumnName("IDTK");

                entity.Property(e => e.MaSv)
                    .IsRequired()
                    .HasColumnName("MaSV")
                    .HasMaxLength(50);

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.Ten).HasMaxLength(50);

                entity.HasOne(d => d.IdtkNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Idtk)
                    .HasConstraintName("FK__Users__IDTK__534D60F1");

                entity.HasOne(d => d.UserTaoNavigation)
                    .WithMany(p => p.InverseUserTaoNavigation)
                    .HasForeignKey(d => d.UserTao)
                    .HasConstraintName("FK__Users__UserTao__14270015");
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasIndex(e => e.MaVaiTro)
                    .HasName("UQ__VaiTro__C24C41CE6CC74208")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MaVaiTro)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TenVaiTro).HasMaxLength(50);
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => new { e.MaCmt, e.MaUser });

                entity.Property(e => e.TimeVote).HasColumnType("datetime");

                entity.HasOne(d => d.MaCmtNavigation)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.MaCmt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vote__MaCmt__19DFD96B");

                entity.HasOne(d => d.MaUserNavigation)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.MaUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vote__MaUser__17F790F9");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
