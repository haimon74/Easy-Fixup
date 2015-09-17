using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupMemberMap : EntityTypeConfiguration<GroupMember>
    {
        public GroupMemberMap()
        {
            // Primary Key
            this.HasKey(t => new { t.g_id, t.u_username });

            // Properties
            this.Property(t => t.g_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.gm_invitedby)
                .HasMaxLength(20);

            this.Property(t => t.gm_joinanswer)
                .IsRequired();

            this.Property(t => t.gm_warn_reason)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("GroupMembers");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gm_type).HasColumnName("gm_type");
            this.Property(t => t.gm_joindate).HasColumnName("gm_joindate");
            this.Property(t => t.gm_invitedby).HasColumnName("gm_invitedby");
            this.Property(t => t.gm_active).HasColumnName("gm_active");
            this.Property(t => t.gm_joinanswer).HasColumnName("gm_joinanswer");
            this.Property(t => t.gm_warned).HasColumnName("gm_warned");
            this.Property(t => t.gm_warn_reason).HasColumnName("gm_warn_reason");
            this.Property(t => t.gm_warn_expirationdate).HasColumnName("gm_warn_expirationdate");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupMembers)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupMembers)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
