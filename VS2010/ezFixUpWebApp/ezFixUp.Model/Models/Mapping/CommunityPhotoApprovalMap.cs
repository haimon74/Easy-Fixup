using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CommunityPhotoApprovalMap : EntityTypeConfiguration<CommunityPhotoApproval>
    {
        public CommunityPhotoApprovalMap()
        {
            // Primary Key
            this.HasKey(t => t.cpa_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CommunityPhotoApproval");
            this.Property(t => t.cpa_id).HasColumnName("cpa_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.cpa_approved).HasColumnName("cpa_approved");
            this.Property(t => t.cpa_timestamp).HasColumnName("cpa_timestamp");

            // Relationships
            this.HasRequired(t => t.Photo)
                .WithMany(t => t.CommunityPhotoApprovals)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.CommunityPhotoApprovals)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
