using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class WinkTypeMap : EntityTypeConfiguration<WinkType>
    {
        public WinkTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.wt_id);

            // Properties
            this.Property(t => t.wt_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.wt_phrase)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("WinkTypes");
            this.Property(t => t.wt_id).HasColumnName("wt_id");
            this.Property(t => t.wt_name).HasColumnName("wt_name");
            this.Property(t => t.wt_phrase).HasColumnName("wt_phrase");
            this.Property(t => t.wt_active).HasColumnName("wt_active");
            this.Property(t => t.wt_category_id).HasColumnName("wt_category_id");

            // Relationships
            this.HasOptional(t => t.WinkCategory)
                .WithMany(t => t.WinkTypes)
                .HasForeignKey(d => d.wt_category_id).WillCascadeOnDelete(false);

        }
    }
}
