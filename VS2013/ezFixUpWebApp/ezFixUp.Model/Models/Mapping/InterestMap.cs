using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class InterestMap : EntityTypeConfiguration<Interest>
    {
        public InterestMap()
        {
            // Primary Key
            this.HasKey(t => t.i_id);

            // Properties
            this.Property(t => t.i_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.i_to_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Interests");
            this.Property(t => t.i_id).HasColumnName("i_id");
            this.Property(t => t.i_from_username).HasColumnName("i_from_username");
            this.Property(t => t.i_to_username).HasColumnName("i_to_username");
            this.Property(t => t.i_date_posted).HasColumnName("i_date_posted");
            this.Property(t => t.i_deleted_by_fromuser).HasColumnName("i_deleted_by_fromuser");
            this.Property(t => t.i_deleted_by_touser).HasColumnName("i_deleted_by_touser");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Interests)
                .HasForeignKey(d => d.i_from_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Interests1)
                .HasForeignKey(d => d.i_to_username).WillCascadeOnDelete(false);

        }
    }
}
