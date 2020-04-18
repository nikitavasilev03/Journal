using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using DomainCore.Models;

namespace DomainCore.Context
{
    public partial class JournalDBContext : DbContext
    {
        public JournalDBContext()
        {
        }

        public JournalDBContext(DbContextOptions<JournalDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Timetable> Timetable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=JournalDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Accounts__46A222CDE66877E9");

                entity.HasIndex(e => e.LoginName)
                    .HasName("UQ__Accounts__F6D56B57DD322742")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasColumnName("account_type")
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreate)
                    .HasColumnName("date_create")
                    .HasColumnType("date");

                entity.Property(e => e.DateEnd)
                    .HasColumnName("date_end")
                    .HasColumnType("date");

                entity.Property(e => e.Hpassword)
                    .IsRequired()
                    .HasColumnName("hpassword")
                    .HasMaxLength(255);

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasColumnName("login_name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Journal>(entity =>
            {
                entity.HasKey(e => e.JourId)
                    .HasName("PK__Journals__BAC66D5A353F7DFD");

                entity.Property(e => e.JourId)
                    .HasColumnName("jour_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.StudentAccountId)
                    .HasColumnName("student_account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.TeacherAccountId)
                    .HasColumnName("teacher_account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.VisitDate)
                    .HasColumnName("visit_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.StudentAccount)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.StudentAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Journals__studen__6FB49575");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Journals__subjec__70A8B9AE");

                entity.HasOne(d => d.TeacherAccount)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.TeacherAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Journals__teache__6EC0713C");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK__Records__BFCFB4DDD08F6A21");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.DateEnd)
                    .HasColumnName("date_end")
                    .HasColumnType("date");

                entity.Property(e => e.DateStart)
                    .HasColumnName("date_start")
                    .HasColumnType("date");

                entity.Property(e => e.IsPassed).HasColumnName("is_passed");

                entity.Property(e => e.NumberVisits)
                    .HasColumnName("number_visits")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.StudentAccountId)
                    .HasColumnName("student_account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .HasColumnType("numeric(5, 0)");

                entity.HasOne(d => d.StudentAccount)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.StudentAccountId)
                    .HasConstraintName("FK__Records__student__6AEFE058");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Records__subject__6BE40491");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Students__46A222CD37554DA1");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.StudentGroup)
                    .HasColumnName("student_group")
                    .HasColumnType("numeric(4, 0)");

                entity.Property(e => e.StudentLname)
                    .HasColumnName("student_lname")
                    .HasMaxLength(255);

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .HasColumnName("student_name")
                    .HasMaxLength(255);

                entity.Property(e => e.StudentSname)
                    .IsRequired()
                    .HasColumnName("student_sname")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Students)
                    .HasForeignKey<Student>(d => d.AccountId)
                    .HasConstraintName("FK__Students__accoun__634EBE90");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectId)
                    .HasName("PK__Subjects__5004F6600506ECAE");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.NeedVisits)
                    .HasColumnName("need_visits")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasColumnName("subject_name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Teachers__46A222CDDD63BD7B");

                entity.Property(e => e.AccountId)
                    .HasColumnName("account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.TeacherLname)
                    .HasColumnName("teacher_lname")
                    .HasMaxLength(255);

                entity.Property(e => e.TeacherName)
                    .IsRequired()
                    .HasColumnName("teacher_name")
                    .HasMaxLength(255);

                entity.Property(e => e.TeacherSname)
                    .IsRequired()
                    .HasColumnName("teacher_sname")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Teachers)
                    .HasForeignKey<Teacher>(d => d.AccountId)
                    .HasConstraintName("FK__Teachers__accoun__662B2B3B");
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.HasKey(e => e.TtId)
                    .HasName("PK__Timetabl__1B50D886B904BC9F");

                entity.Property(e => e.TtId)
                    .HasColumnName("tt_id")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.RecordId)
                    .HasColumnName("record_id")
                    .HasColumnType("numeric(5, 0)");

                entity.Property(e => e.TeacherAccountId)
                    .HasColumnName("teacher_account_id")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.TtNumLesson)
                    .HasColumnName("tt_num_lesson")
                    .HasColumnType("numeric(1, 0)");

                entity.Property(e => e.TtWeekDay)
                    .HasColumnName("tt_week_day")
                    .HasColumnType("numeric(1, 0)");

                entity.HasOne(d => d.Record)
                    .WithMany(p => p.Timetable)
                    .HasForeignKey(d => d.RecordId)
                    .HasConstraintName("FK__Timetable__recor__73852659");

                entity.HasOne(d => d.TeacherAccount)
                    .WithMany(p => p.Timetable)
                    .HasForeignKey(d => d.TeacherAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Timetable__teach__74794A92");
            });

            modelBuilder.HasSequence("SEQ_Accounts");

            modelBuilder.HasSequence("SEQ_Journals");

            modelBuilder.HasSequence("SEQ_Records");

            modelBuilder.HasSequence("SEQ_Subjects");

            modelBuilder.HasSequence("SEQ_Timetable");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
