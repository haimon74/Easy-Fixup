using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EmailTemplateMap : EntityTypeConfiguration<EmailTemplate>
    {
        public EmailTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.et_name);

            // Properties
            this.Property(t => t.et_name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.et_subject)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.et_body)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("EmailTemplates");
            this.Property(t => t.et_name).HasColumnName("et_name");
            this.Property(t => t.et_subject).HasColumnName("et_subject");
            this.Property(t => t.et_body).HasColumnName("et_body");
        }
    }
}
