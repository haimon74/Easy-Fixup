using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ContentPageMap : EntityTypeConfiguration<ContentPage>
    {
        public ContentPageMap()
        {
            // Primary Key
            this.HasKey(t => t.cp_id);

            // Properties
            this.Property(t => t.cp_title)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.cp_content)
                .IsRequired();

            this.Property(t => t.cp_url)
                .HasMaxLength(250);

            this.Property(t => t.cp_metadescription)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.cp_metakeyword)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.cp_urlrewrite)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("ContentPage");
            this.Property(t => t.cp_id).HasColumnName("cp_id");
            this.Property(t => t.cp_title).HasColumnName("cp_title");
            this.Property(t => t.cp_content).HasColumnName("cp_content");
            this.Property(t => t.cp_header_position).HasColumnName("cp_header_position");
            this.Property(t => t.cp_footer_position).HasColumnName("cp_footer_position");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.cp_visiblefor).HasColumnName("cp_visiblefor");
            this.Property(t => t.cp_url).HasColumnName("cp_url");
            this.Property(t => t.cp_metadescription).HasColumnName("cp_metadescription");
            this.Property(t => t.cp_metakeyword).HasColumnName("cp_metakeyword");
            this.Property(t => t.cp_urlrewrite).HasColumnName("cp_urlrewrite");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.ContentPages)
                .HasForeignKey(d => d.l_id).WillCascadeOnDelete(false);

        }
    }
}
