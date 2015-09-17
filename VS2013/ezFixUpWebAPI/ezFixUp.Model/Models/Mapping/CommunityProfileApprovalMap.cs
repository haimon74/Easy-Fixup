using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CommunityProfileApprovalMap : EntityTypeConfiguration<CommunityProfileApproval>
    {
        public CommunityProfileApprovalMap()
        {
            // Primary Key
            this.HasKey(t => t.cpa_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.cpa_approvedby)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CommunityProfileApproval");
            this.Property(t => t.cpa_id).HasColumnName("cpa_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.cpa_approvedby).HasColumnName("cpa_approvedby");
            this.Property(t => t.cpa_approved).HasColumnName("cpa_approved");
            this.Property(t => t.cpa_timestamp).HasColumnName("cpa_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.CommunityProfileApprovals)
                .HasForeignKey(d => d.cpa_approvedby).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.CommunityProfileApprovals1)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
