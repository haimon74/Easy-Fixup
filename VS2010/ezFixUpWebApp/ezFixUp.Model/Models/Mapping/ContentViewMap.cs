using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ContentViewMap : EntityTypeConfiguration<ContentView>
    {
        public ContentViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cv_key, t.l_id });

            // Properties
            this.Property(t => t.cv_key)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.l_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.cv_content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ContentViews");
            this.Property(t => t.cv_key).HasColumnName("cv_key");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.cv_content).HasColumnName("cv_content");
        }
    }
}
