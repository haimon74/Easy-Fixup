using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AdMap : EntityTypeConfiguration<Ad>
    {
        public AdMap()
        {
            // Primary Key
            this.HasKey(t => t.a_id);

            // Properties
            this.Property(t => t.a_postedby)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.a_location)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.a_subject)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.a_description)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Ads");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.ac_id).HasColumnName("ac_id");
            this.Property(t => t.a_postedby).HasColumnName("a_postedby");
            this.Property(t => t.a_date).HasColumnName("a_date");
            this.Property(t => t.a_expirationdate).HasColumnName("a_expirationdate");
            this.Property(t => t.a_location).HasColumnName("a_location");
            this.Property(t => t.a_subject).HasColumnName("a_subject");
            this.Property(t => t.a_description).HasColumnName("a_description");
            this.Property(t => t.a_approved).HasColumnName("a_approved");

            // Relationships
            this.HasRequired(t => t.AdsCategory)
                .WithMany(t => t.Ads)
                .HasForeignKey(d => d.ac_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Ads)
                .HasForeignKey(d => d.a_postedby).WillCascadeOnDelete(false);

        }
    }
}
