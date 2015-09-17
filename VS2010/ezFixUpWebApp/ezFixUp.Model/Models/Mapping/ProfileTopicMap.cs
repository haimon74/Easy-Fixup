using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ProfileTopicMap : EntityTypeConfiguration<ProfileTopic>
    {
        public ProfileTopicMap()
        {
            // Primary Key
            this.HasKey(t => t.pt_id);

            // Properties
            this.Property(t => t.pt_name)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("ProfileTopics");
            this.Property(t => t.pt_id).HasColumnName("pt_id");
            this.Property(t => t.pt_name).HasColumnName("pt_name");
            this.Property(t => t.pt_priority).HasColumnName("pt_priority");
            this.Property(t => t.pt_editcolumns).HasColumnName("pt_editcolumns");
            this.Property(t => t.pt_viewcolumns).HasColumnName("pt_viewcolumns");
        }
    }
}
