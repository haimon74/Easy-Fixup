using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PendingGuidMap : EntityTypeConfiguration<PendingGuid>
    {
        public PendingGuidMap()
        {
            // Primary Key
            this.HasKey(t => t.pg_guid);

            // Properties
            this.Property(t => t.pg_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pg_guid)
                .IsRequired()
                .HasMaxLength(36);

            // Table & Column Mappings
            this.ToTable("PendingGuids");
            this.Property(t => t.pg_username).HasColumnName("pg_username");
            this.Property(t => t.pg_guid).HasColumnName("pg_guid");
        }
    }
}
