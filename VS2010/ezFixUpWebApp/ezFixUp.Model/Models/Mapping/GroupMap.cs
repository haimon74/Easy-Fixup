using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.g_id);

            // Properties
            this.Property(t => t.g_name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.g_description)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.g_owner)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.g_jointerms)
                .IsRequired();

            this.Property(t => t.g_joinquestion)
                .IsRequired();

            this.Property(t => t.g_autojoincountry)
                .HasMaxLength(100);

            this.Property(t => t.g_autojoinregion)
                .HasMaxLength(100);

            this.Property(t => t.g_autojoincity)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Groups");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.g_name).HasColumnName("g_name");
            this.Property(t => t.g_description).HasColumnName("g_description");
            this.Property(t => t.g_icon).HasColumnName("g_icon");
            this.Property(t => t.g_datecreated).HasColumnName("g_datecreated");
            this.Property(t => t.g_approved).HasColumnName("g_approved");
            this.Property(t => t.g_accesslevel).HasColumnName("g_accesslevel");
            this.Property(t => t.g_owner).HasColumnName("g_owner");
            this.Property(t => t.g_activemembers).HasColumnName("g_activemembers");
            this.Property(t => t.g_jointerms).HasColumnName("g_jointerms");
            this.Property(t => t.g_joinquestion).HasColumnName("g_joinquestion");
            this.Property(t => t.g_permissions).HasColumnName("g_permissions");
            this.Property(t => t.g_minage).HasColumnName("g_minage");
            this.Property(t => t.g_autojoin).HasColumnName("g_autojoin");
            this.Property(t => t.g_autojoincountry).HasColumnName("g_autojoincountry");
            this.Property(t => t.g_autojoinregion).HasColumnName("g_autojoinregion");
            this.Property(t => t.g_autojoincity).HasColumnName("g_autojoincity");
        }
    }
}
