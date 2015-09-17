using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ProfileViewMap : EntityTypeConfiguration<ProfileView>
    {
        public ProfileViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.pv_viewer, t.pv_viewed });

            // Properties
            this.Property(t => t.pv_viewer)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pv_viewed)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ProfileViews");
            this.Property(t => t.pv_viewer).HasColumnName("pv_viewer");
            this.Property(t => t.pv_viewed).HasColumnName("pv_viewed");
            this.Property(t => t.pv_timestamp).HasColumnName("pv_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.ProfileViews)
                .HasForeignKey(d => d.pv_viewer).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.ProfileViews1)
                .HasForeignKey(d => d.pv_viewed).WillCascadeOnDelete(false);

        }
    }
}
