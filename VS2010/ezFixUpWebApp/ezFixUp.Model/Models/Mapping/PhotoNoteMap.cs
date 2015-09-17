using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoNoteMap : EntityTypeConfiguration<PhotoNote>
    {
        public PhotoNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.pn_id);

            // Properties
            this.Property(t => t.u_username)
                .HasMaxLength(20);

            this.Property(t => t.pn_notes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("PhotoNotes");
            this.Property(t => t.pn_id).HasColumnName("pn_id");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pn_notes).HasColumnName("pn_notes");
            this.Property(t => t.pn_timestamp).HasColumnName("pn_timestamp");
            this.Property(t => t.pn_x).HasColumnName("pn_x");
            this.Property(t => t.pn_y).HasColumnName("pn_y");
            this.Property(t => t.pn_width).HasColumnName("pn_width");
            this.Property(t => t.pn_height).HasColumnName("pn_height");

            // Relationships
            this.HasRequired(t => t.Photo)
                .WithMany(t => t.PhotoNotes)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);
            this.HasOptional(t => t.User)
                .WithMany(t => t.PhotoNotes)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
