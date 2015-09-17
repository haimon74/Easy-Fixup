using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class MessagesTypeMap : EntityTypeConfiguration<MessagesType>
    {
        public MessagesTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.mt_id);

            // Properties
            this.Property(t => t.mt_name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MessagesTypes");
            this.Property(t => t.mt_id).HasColumnName("mt_id");
            this.Property(t => t.mt_name).HasColumnName("mt_name");
            this.Property(t => t.mt_active).HasColumnName("mt_active");
        }
    }
}
