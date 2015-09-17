using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupEventMap : EntityTypeConfiguration<GroupEvent>
    {
        public GroupEventMap()
        {
            // Primary Key
            this.HasKey(t => t.ge_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ge_title)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ge_description)
                .IsRequired();

            this.Property(t => t.ge_location)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("GroupEvents");
            this.Property(t => t.ge_id).HasColumnName("ge_id");
            this.Property(t => t.g_id).HasColumnName("g_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ge_title).HasColumnName("ge_title");
            this.Property(t => t.ge_description).HasColumnName("ge_description");
            this.Property(t => t.ge_image).HasColumnName("ge_image");
            this.Property(t => t.ge_date).HasColumnName("ge_date");
            this.Property(t => t.ge_location).HasColumnName("ge_location");

            // Relationships
            this.HasMany(t => t.Users)
                .WithMany(t => t.GroupEvents1)
                .Map(m =>
                    {
                        m.ToTable("GroupEventsAttenders");
                        m.MapLeftKey("ge_id");
                        m.MapRightKey("u_username");
                    });

            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupEvents)
                .HasForeignKey(d => d.g_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupEvents)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
