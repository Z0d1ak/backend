using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace gpn
{
    public partial class GPNContext : DbContext
    {
        public GPNContext()
        {
        }

        public GPNContext(DbContextOptions<GPNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArrivalOperation> ArrivalOperations { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<OperationDeadline> OperationDeadlines { get; set; }
        public virtual DbSet<OpertationType> OpertationTypes { get; set; }
        public virtual DbSet<SlaRule> SlaRules { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<FileMetadata> Files { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=connect.ineutov.me; Database=GPN; Integrated Security=false; User ID=sa; Password=P@ssw0rd;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_100_BIN");

            modelBuilder.Entity<ArrivalOperation>(entity =>
            {
                entity.ToTable("ArrivalOperation");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.LogisticCompany)
                    .HasMaxLength(255)
                    .HasColumnName("logisticCompany");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.ArrivalOperation)
                    .HasForeignKey<ArrivalOperation>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ArrivalOpera__id__4AB81AF0");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Number)
                    .HasName("PK__Equipmen__FD291E4095624219");

                entity.Property(e => e.Number)
                    .HasMaxLength(255)
                    .HasColumnName("number");

                entity.Property(e => e.ComapnyId).HasColumnName("comapnyID");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ParentId)
                    .HasMaxLength(255)
                    .HasColumnName("parentID");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .HasColumnName("state");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.HasOne(d => d.Comapny)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.ComapnyId)
                    .HasConstraintName("FK__Equipment__comap__440B1D61");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__Equipment__paren__44FF419A");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.FileID)
                    .HasColumnName("fileId");

                entity.HasOne(x => x.File);

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(255)
                    .HasColumnName("equipmentNumber");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Performer)
                    .HasMaxLength(255)
                    .HasColumnName("performer");

                entity.Property(e => e.PostponedTime).HasColumnName("postponedTime");

                

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.EquipmentNumberNavigation)
                    .WithMany(p => p.Operations)
                    .HasForeignKey(d => d.EquipmentNumber)
                    .HasConstraintName("FK__Operation__equip__48CFD27E");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Operations)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Operation__typeI__49C3F6B7");
            });

            modelBuilder.Entity<OperationDeadline>(entity =>
            {
                entity.HasKey(e => new { e.EquipmentNumber, e.TypeId })
                    .HasName("PK__Operatio__9EF368CB25B1225D");

                entity.Property(e => e.EquipmentNumber)
                    .HasMaxLength(255)
                    .HasColumnName("equipmentNumber");

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.Property(e => e.OperationId).HasColumnName("operationID");

                entity.HasOne(d => d.EquipmentNumberNavigation)
                    .WithMany(p => p.OperationDeadlines)
                    .HasForeignKey(d => d.EquipmentNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Operation__equip__45F365D3");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.OperationDeadlines)
                    .HasForeignKey(d => d.OperationId)
                    .HasConstraintName("FK__Operation__opera__46E78A0C");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.OperationDeadlines)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Operation__typeI__47DBAE45");
            });

            modelBuilder.Entity<OpertationType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<SlaRule>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.NextTypeId).HasColumnName("nextTypeID");

                entity.Property(e => e.OptionType)
                    .HasMaxLength(255)
                    .HasColumnName("optionType");

                entity.Property(e => e.TypeId).HasColumnName("typeID");

                entity.HasOne(d => d.NextType)
                    .WithMany(p => p.SlaRuleNextTypes)
                    .HasForeignKey(d => d.NextTypeId)
                    .HasConstraintName("FK__SlaRules__nextTy__4CA06362");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.SlaRuleTypes)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__SlaRules__typeID__4BAC3F29");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComapnyId).HasColumnName("comapnyID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.HasOne(d => d.Comapny)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ComapnyId)
                    .HasConstraintName("FK__User__comapnyID__4D94879B");
            });

            modelBuilder.Entity<FileMetadata>().HasKey(x => x.ID);



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
