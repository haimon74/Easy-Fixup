using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AdCommentMap : EntityTypeConfiguration<AdComment>
    {
        public AdCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.ac_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ac_comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AdComments");
            this.Property(t => t.ac_id).HasColumnName("ac_id");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ac_comment).HasColumnName("ac_comment");
            this.Property(t => t.ac_date).HasColumnName("ac_date");

            // Relationships
            this.HasRequired(t => t.Ad)
                .WithMany(t => t.AdComments)
                .HasForeignKey(d => d.a_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.AdComments)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
