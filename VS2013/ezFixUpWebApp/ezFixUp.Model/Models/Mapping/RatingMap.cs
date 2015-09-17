using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class RatingMap : EntityTypeConfiguration<Rating>
    {
        public RatingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.r_fromusername, t.r_tousername });

            // Properties
            this.Property(t => t.r_fromusername)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.r_tousername)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.r_ip)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Ratings");
            this.Property(t => t.r_fromusername).HasColumnName("r_fromusername");
            this.Property(t => t.r_tousername).HasColumnName("r_tousername");
            this.Property(t => t.r_rating).HasColumnName("r_rating");
            this.Property(t => t.r_timestamp).HasColumnName("r_timestamp");
            this.Property(t => t.r_ip).HasColumnName("r_ip");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Ratings)
                .HasForeignKey(d => d.r_fromusername).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Ratings1)
                .HasForeignKey(d => d.r_tousername).WillCascadeOnDelete(false);

        }
    }
}
