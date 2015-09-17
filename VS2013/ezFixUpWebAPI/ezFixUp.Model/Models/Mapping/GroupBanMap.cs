using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupBanMap : EntityTypeConfiguration<GroupBan>
    {
        public GroupBanMap()
        {
            // Primary Key
            this.HasKey(t => t.gb_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("GroupBans");
            this.Property(t => t.gb_id).HasColumnName("gb_id");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gb_expires).HasColumnName("gb_expires");
            this.Property(t => t.gb_date).HasColumnName("gb_date");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupBans)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupBans)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
