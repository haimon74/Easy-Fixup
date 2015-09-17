using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class LanguageMap : EntityTypeConfiguration<Language>
    {
        public LanguageMap()
        {
            // Primary Key
            this.HasKey(t => t.l_id);

            // Properties
            this.Property(t => t.l_language)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.l_browser_languages)
                .HasMaxLength(100);

            this.Property(t => t.l_ipcountries)
                .HasMaxLength(200);

            this.Property(t => t.l_theme)
                .HasMaxLength(100);

            this.Property(t => t.l_dir)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Languages");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.l_language).HasColumnName("l_language");
            this.Property(t => t.l_active).HasColumnName("l_active");
            this.Property(t => t.l_predefined).HasColumnName("l_predefined");
            this.Property(t => t.l_order).HasColumnName("l_order");
            this.Property(t => t.l_browser_languages).HasColumnName("l_browser_languages");
            this.Property(t => t.l_ipcountries).HasColumnName("l_ipcountries");
            this.Property(t => t.l_theme).HasColumnName("l_theme");
            this.Property(t => t.l_dir).HasColumnName("l_dir");
        }
    }
}
