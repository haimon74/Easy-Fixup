using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EcardMap : EntityTypeConfiguration<Ecard>
    {
        public EcardMap()
        {
            // Primary Key
            this.HasKey(t => t.ec_id);

            // Properties
            this.Property(t => t.ec_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ec_to_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ec_message)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("Ecards");
            this.Property(t => t.ec_id).HasColumnName("ec_id");
            this.Property(t => t.ect_id).HasColumnName("ect_id");
            this.Property(t => t.ec_from_username).HasColumnName("ec_from_username");
            this.Property(t => t.ec_to_username).HasColumnName("ec_to_username");
            this.Property(t => t.ec_date).HasColumnName("ec_date");
            this.Property(t => t.ec_message).HasColumnName("ec_message");
            this.Property(t => t.ec_deleted_by_fromuser).HasColumnName("ec_deleted_by_fromuser");
            this.Property(t => t.ec_deleted_by_touser).HasColumnName("ec_deleted_by_touser");
            this.Property(t => t.ec_is_opened).HasColumnName("ec_is_opened");

            // Relationships
            this.HasRequired(t => t.EcardType)
                .WithMany(t => t.Ecards)
                .HasForeignKey(d => d.ect_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Ecards)
                .HasForeignKey(d => d.ec_from_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Ecards1)
                .HasForeignKey(d => d.ec_to_username).WillCascadeOnDelete(false);

        }
    }
}
