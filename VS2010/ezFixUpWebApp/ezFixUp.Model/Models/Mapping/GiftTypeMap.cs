using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GiftTypeMap : EntityTypeConfiguration<GiftType>
    {
        public GiftTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.gt_id);

            // Properties
            this.Property(t => t.gt_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.gt_phrase)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("GiftTypes");
            this.Property(t => t.gt_id).HasColumnName("gt_id");
            this.Property(t => t.gt_name).HasColumnName("gt_name");
            this.Property(t => t.gt_phrase).HasColumnName("gt_phrase");
            this.Property(t => t.gt_content).HasColumnName("gt_content");
            this.Property(t => t.gt_type).HasColumnName("gt_type");
            this.Property(t => t.gt_active).HasColumnName("gt_active");
            this.Property(t => t.gt_category_id).HasColumnName("gt_category_id");
            this.Property(t => t.gt_price).HasColumnName("gt_price");

            // Relationships
            this.HasOptional(t => t.GiftCategory)
                .WithMany(t => t.GiftTypes)
                .HasForeignKey(d => d.gt_category_id).WillCascadeOnDelete(false);

        }
    }
}
