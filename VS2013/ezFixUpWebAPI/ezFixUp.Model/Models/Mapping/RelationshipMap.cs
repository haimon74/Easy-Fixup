using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class RelationshipMap : EntityTypeConfiguration<Relationship>
    {
        public RelationshipMap()
        {
            // Primary Key
            this.HasKey(t => t.r_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.r_username)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Relationships");
            this.Property(t => t.r_id).HasColumnName("r_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.r_username).HasColumnName("r_username");
            this.Property(t => t.r_type).HasColumnName("r_type");
            this.Property(t => t.r_pendingtype).HasColumnName("r_pendingtype");
            this.Property(t => t.r_accepted).HasColumnName("r_accepted");
            this.Property(t => t.r_timestamp).HasColumnName("r_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Relationships)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);
            this.HasOptional(t => t.User1)
                .WithMany(t => t.Relationships1)
                .HasForeignKey(d => d.r_username).WillCascadeOnDelete(false);

        }
    }
}
