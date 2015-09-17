using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GiftsMessageMap : EntityTypeConfiguration<GiftsMessage>
    {
        public GiftsMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.gm_id);

            // Properties
            // Table & Column Mappings
            this.ToTable("GiftsMessages");
            this.Property(t => t.gm_id).HasColumnName("gm_id");
            this.Property(t => t.gm_gift_id).HasColumnName("gm_gift_id");
            this.Property(t => t.gm_message_id).HasColumnName("gm_message_id");

            // Relationships
            this.HasRequired(t => t.GiftType)
                .WithMany(t => t.GiftsMessages)
                .HasForeignKey(d => d.gm_gift_id).WillCascadeOnDelete(false);

        }
    }
}
