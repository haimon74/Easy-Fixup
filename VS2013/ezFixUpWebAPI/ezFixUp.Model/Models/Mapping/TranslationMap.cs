using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class TranslationMap : EntityTypeConfiguration<Translation>
    {
        public TranslationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.l_id, t.t_key, t.t_adminpanel });

            // Properties
            this.Property(t => t.l_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.t_key)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.t_adminpanel)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.t_value)
                .IsRequired()
                .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("Translations");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.t_key).HasColumnName("t_key");
            this.Property(t => t.t_adminpanel).HasColumnName("t_adminpanel");
            this.Property(t => t.t_value).HasColumnName("t_value");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.Translations)
                .HasForeignKey(d => d.l_id).WillCascadeOnDelete(false);

        }
    }
}
