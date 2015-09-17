using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoRatingMap : EntityTypeConfiguration<PhotoRating>
    {
        public PhotoRatingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.pr_fromusername, t.p_id });

            // Properties
            this.Property(t => t.pr_fromusername)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.p_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PhotoRatings");
            this.Property(t => t.pr_fromusername).HasColumnName("pr_fromusername");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.pr_rating).HasColumnName("pr_rating");
            this.Property(t => t.pr_timestamp).HasColumnName("pr_timestamp");

            // Relationships
            this.HasRequired(t => t.Photo)
                .WithMany(t => t.PhotoRatings)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoRatings)
                .HasForeignKey(d => d.pr_fromusername).WillCascadeOnDelete(false);

        }
    }
}
