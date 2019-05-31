using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using backend.jingle.net.Models;

namespace backend.jingle.net.Models
{
    public partial class JINGLDBContext : DbContext
    {
        public JINGLDBContext()
        {
        }

        public JINGLDBContext(DbContextOptions<JINGLDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConfigurationTable> ConfigurationTable { get; set; }
        public virtual DbSet<LandingRegistration> LandingRegistration { get; set; }
        public virtual DbSet<UserAuth> UserAuth { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=dbsophieparis.cmztxkrk8l6n.ap-southeast-1.rds.amazonaws.com;Database=JINGLDB;Persist Security Info=True;User ID=jingluser;Password=jingluser_p4ss;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigurationTable>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfigName).HasMaxLength(150);

                entity.Property(e => e.ConfigValue).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<LandingRegistrationRequest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FacebookUrl)
                    .HasColumnName("FacebookURL")
                    .HasMaxLength(150);

                entity.Property(e => e.FileUrl).HasColumnName("FileURL");

                entity.Property(e => e.GoogleUrl)
                    .HasColumnName("GoogleURL")
                    .HasMaxLength(150);

                entity.Property(e => e.InstagramUrl)
                    .HasColumnName("InstagramURL")
                    .HasMaxLength(150);

                entity.Property(e => e.LinkedInUrl)
                    .HasColumnName("LinkedInURL")
                    .HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.RegisteredIp)
                    .HasColumnName("RegisteredIP")
                    .HasMaxLength(50);

                entity.Property(e => e.RegisteredUserAgent).HasMaxLength(50);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.SessionId).HasColumnName("SessionID");

                entity.Property(e => e.SocialMedia).HasMaxLength(255);

                entity.Property(e => e.TwitterUrl)
                    .HasColumnName("TwitterURL")
                    .HasMaxLength(150);

                entity.Property(e => e.UniqueKey).HasMaxLength(50);
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LoginType).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Email).HasColumnName("Email");
            });
        }

        public DbSet<backend.jingle.net.Models.AdminAuth> AdminAuth { get; set; }
        public DbSet<backend.jingle.net.Models.ContactMessage> ContactMessage { get; set; }
    }
}
