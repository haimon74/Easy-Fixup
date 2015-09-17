using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class TemplateMap : EntityTypeConfiguration<Template>
    {
        public TemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.t_key);

            // Properties
            this.Property(t => t.t_key)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.t_value)
                .IsRequired();

            this.Property(t => t.t_description)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Templates");
            this.Property(t => t.t_key).HasColumnName("t_key");
            this.Property(t => t.t_value).HasColumnName("t_value");
            this.Property(t => t.t_description).HasColumnName("t_description");
        }
    }
}
