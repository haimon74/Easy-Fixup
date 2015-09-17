using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BlockedUserMap : EntityTypeConfiguration<BlockedUser>
    {
        public BlockedUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.bu_userblocker, t.bu_blockeduser });

            // Properties
            this.Property(t => t.bu_userblocker)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.bu_blockeduser)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("BlockedUsers");
            this.Property(t => t.bu_userblocker).HasColumnName("bu_userblocker");
            this.Property(t => t.bu_blockeduser).HasColumnName("bu_blockeduser");
            this.Property(t => t.bu_blockdate).HasColumnName("bu_blockdate");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.BlockedUsers)
                .HasForeignKey(d => d.bu_userblocker).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.BlockedUsers1)
                .HasForeignKey(d => d.bu_blockeduser).WillCascadeOnDelete(false);

        }
    }
}
