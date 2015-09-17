using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BannerCodeMap : EntityTypeConfiguration<BannerCode>
    {
        public BannerCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.bc_id);

            // Properties
            this.Property(t => t.bc_code)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BannerCodes");
            this.Property(t => t.bc_id).HasColumnName("bc_id");
            this.Property(t => t.bc_position).HasColumnName("bc_position");
            this.Property(t => t.bc_priority).HasColumnName("bc_priority");
            this.Property(t => t.bc_target).HasColumnName("bc_target");
            this.Property(t => t.bc_code).HasColumnName("bc_code");
        }
    }
}
