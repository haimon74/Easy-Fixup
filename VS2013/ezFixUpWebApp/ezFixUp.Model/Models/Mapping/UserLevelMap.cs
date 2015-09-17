using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class UserLevelMap : EntityTypeConfiguration<UserLevel>
    {
        public UserLevelMap()
        {
            // Primary Key
            this.HasKey(t => t.ul_id);

            // Properties
            this.Property(t => t.ul_name)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("UserLevels");
            this.Property(t => t.ul_id).HasColumnName("ul_id");
            this.Property(t => t.ul_name).HasColumnName("ul_name");
            this.Property(t => t.ul_minscore).HasColumnName("ul_minscore");
            this.Property(t => t.ul_restrictions).HasColumnName("ul_restrictions");
        }
    }
}
