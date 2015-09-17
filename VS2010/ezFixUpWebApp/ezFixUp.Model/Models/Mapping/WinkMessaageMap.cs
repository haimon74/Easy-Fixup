using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class WinkMessaageMap : EntityTypeConfiguration<WinkMessaage>
    {
        public WinkMessaageMap()
        {
            // Primary Key
            this.HasKey(t => t.wm_id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WinkMessaages");
            this.Property(t => t.wm_id).HasColumnName("wm_id");
            this.Property(t => t.wm_message_id).HasColumnName("wm_message_id");
            this.Property(t => t.wm_wink_id).HasColumnName("wm_wink_id");

            // Relationships
            this.HasRequired(t => t.WinkType)
                .WithMany(t => t.WinkMessaages)
                .HasForeignKey(d => d.wm_wink_id).WillCascadeOnDelete(false);

        }
    }
}
