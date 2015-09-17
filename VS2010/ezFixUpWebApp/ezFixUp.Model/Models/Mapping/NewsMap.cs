using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class NewsMap : EntityTypeConfiguration<News>
    {
        public NewsMap()
        {
            // Primary Key
            this.HasKey(t => t.n_id);

            // Properties
            this.Property(t => t.n_text)
                .IsRequired();

            this.Property(t => t.n_title)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("News");
            this.Property(t => t.n_id).HasColumnName("n_id");
            this.Property(t => t.n_date).HasColumnName("n_date");
            this.Property(t => t.n_text).HasColumnName("n_text");
            this.Property(t => t.n_title).HasColumnName("n_title");
            this.Property(t => t.l_id).HasColumnName("l_id");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.News)
                .HasForeignKey(d => d.l_id).WillCascadeOnDelete(false);

        }
    }
}
