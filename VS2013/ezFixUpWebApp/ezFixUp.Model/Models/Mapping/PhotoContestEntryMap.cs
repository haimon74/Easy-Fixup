using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoContestEntryMap : EntityTypeConfiguration<PhotoContestEntry>
    {
        public PhotoContestEntryMap()
        {
            // Primary Key
            this.HasKey(t => t.pce_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PhotoContestEntries");
            this.Property(t => t.pce_id).HasColumnName("pce_id");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.p_id).HasColumnName("p_id");

            // Relationships
            this.HasRequired(t => t.PhotoContest)
                .WithMany(t => t.PhotoContestEntries)
                .HasForeignKey(d => d.pc_id).WillCascadeOnDelete(false);

        }
    }
}
