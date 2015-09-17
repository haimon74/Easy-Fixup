using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.c_id);

            // Properties
            this.Property(t => t.c_name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Categories");
            this.Property(t => t.c_id).HasColumnName("c_id");
            this.Property(t => t.c_name).HasColumnName("c_name");
            this.Property(t => t.c_order).HasColumnName("c_order");
            this.Property(t => t.c_userscancreategroups).HasColumnName("c_userscancreategroups");

            // Relationships
            this.HasMany(t => t.Groups)
                .WithMany(t => t.Categories)
                .Map(m =>
                    {
                        m.ToTable("GroupsCategories");
                        m.MapLeftKey("c_id");
                        m.MapRightKey("g_id");
                    });


        }
    }
}
