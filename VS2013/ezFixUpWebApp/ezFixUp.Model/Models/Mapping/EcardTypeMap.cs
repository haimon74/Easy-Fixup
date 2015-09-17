using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EcardTypeMap : EntityTypeConfiguration<EcardType>
    {
        public EcardTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ect_id);

            // Properties
            this.Property(t => t.ect_name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ect_phrase)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("EcardTypes");
            this.Property(t => t.ect_id).HasColumnName("ect_id");
            this.Property(t => t.ect_name).HasColumnName("ect_name");
            this.Property(t => t.ect_phrase).HasColumnName("ect_phrase");
            this.Property(t => t.ect_content).HasColumnName("ect_content");
            this.Property(t => t.ect_type).HasColumnName("ect_type");
            this.Property(t => t.ect_active).HasColumnName("ect_active");
            this.Property(t => t.ect_category_id).HasColumnName("ect_category_id");

            // Relationships
            this.HasOptional(t => t.ECardCategory)
                .WithMany(t => t.EcardTypes)
                .HasForeignKey(d => d.ect_category_id).WillCascadeOnDelete(false);
            this.HasOptional(t => t.GiftCategory)
                .WithMany(t => t.EcardTypes)
                .HasForeignKey(d => d.ect_category_id).WillCascadeOnDelete(false);

        }
    }
}
