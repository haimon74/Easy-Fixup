using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ScheduledAnnouncementMap : EntityTypeConfiguration<ScheduledAnnouncement>
    {
        public ScheduledAnnouncementMap()
        {
            // Primary Key
            this.HasKey(t => t.sa_id);

            // Properties
            this.Property(t => t.sa_name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.sa_subject)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.sa_body)
                .IsRequired();

            this.Property(t => t.sa_country)
                .HasMaxLength(100);

            this.Property(t => t.sa_region)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ScheduledAnnouncements");
            this.Property(t => t.sa_id).HasColumnName("sa_id");
            this.Property(t => t.sa_name).HasColumnName("sa_name");
            this.Property(t => t.sa_subject).HasColumnName("sa_subject");
            this.Property(t => t.sa_body).HasColumnName("sa_body");
            this.Property(t => t.sa_gender).HasColumnName("sa_gender");
            this.Property(t => t.sa_paidmember).HasColumnName("sa_paidmember");
            this.Property(t => t.sa_hasphotos).HasColumnName("sa_hasphotos");
            this.Property(t => t.sa_hasprofile).HasColumnName("sa_hasprofile");
            this.Property(t => t.sa_languageid).HasColumnName("sa_languageid");
            this.Property(t => t.sa_country).HasColumnName("sa_country");
            this.Property(t => t.sa_region).HasColumnName("sa_region");
            this.Property(t => t.sa_type).HasColumnName("sa_type");
            this.Property(t => t.sa_date).HasColumnName("sa_date");
            this.Property(t => t.sa_days).HasColumnName("sa_days");
        }
    }
}
