using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoContestVoteMap : EntityTypeConfiguration<PhotoContestVote>
    {
        public PhotoContestVoteMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.pce_id_picked, t.pce_id_nonpicked });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pce_id_picked)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.pce_id_nonpicked)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PhotoContestVotes");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pce_id_picked).HasColumnName("pce_id_picked");
            this.Property(t => t.pce_id_nonpicked).HasColumnName("pce_id_nonpicked");

            // Relationships
            this.HasRequired(t => t.PhotoContestEntry)
                .WithMany(t => t.PhotoContestVotes)
                .HasForeignKey(d => d.pce_id_nonpicked).WillCascadeOnDelete(false);
            this.HasRequired(t => t.PhotoContestEntry1)
                .WithMany(t => t.PhotoContestVotes1)
                .HasForeignKey(d => d.pce_id_picked).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoContestVotes)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
