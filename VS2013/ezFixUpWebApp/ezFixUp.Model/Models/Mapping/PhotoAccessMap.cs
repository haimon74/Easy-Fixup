using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoAccessMap : EntityTypeConfiguration<PhotoAccess>
    {
        public PhotoAccessMap()
        {
            // Primary Key
            this.HasKey(t => new { t.pa_photoowner, t.pa_photoviewer });

            // Properties
            this.Property(t => t.pa_photoowner)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pa_photoviewer)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PhotoAccess");
            this.Property(t => t.pa_photoowner).HasColumnName("pa_photoowner");
            this.Property(t => t.pa_photoviewer).HasColumnName("pa_photoviewer");
            this.Property(t => t.pa_dateaccessgranted).HasColumnName("pa_dateaccessgranted");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoAccesses)
                .HasForeignKey(d => d.pa_photoowner).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.PhotoAccesses1)
                .HasForeignKey(d => d.pa_photoviewer).WillCascadeOnDelete(false);

        }
    }
}
