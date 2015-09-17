using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoContestRankMap : EntityTypeConfiguration<PhotoContestRank>
    {
        public PhotoContestRankMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.pc_id, t.pce_id });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.pc_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.pce_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PhotoContestRanks");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.pce_id).HasColumnName("pce_id");
            this.Property(t => t.pcr_value).HasColumnName("pcr_value");

            // Relationships
            this.HasRequired(t => t.PhotoContestEntry)
                .WithMany(t => t.PhotoContestRanks)
                .HasForeignKey(d => d.pce_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.PhotoContest)
                .WithMany(t => t.PhotoContestRanks)
                .HasForeignKey(d => d.pc_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.PhotoContestRanks)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
