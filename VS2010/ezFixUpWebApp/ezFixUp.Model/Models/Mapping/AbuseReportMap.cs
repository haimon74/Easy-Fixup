using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AbuseReportMap : EntityTypeConfiguration<AbuseReport>
    {
        public AbuseReportMap()
        {
            // Primary Key
            this.HasKey(t => t.ar_id);

            // Properties
            this.Property(t => t.ar_reportedby)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ar_report)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.ar_reporteduser)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("AbuseReports");
            this.Property(t => t.ar_id).HasColumnName("ar_id");
            this.Property(t => t.ar_reportedby).HasColumnName("ar_reportedby");
            this.Property(t => t.ar_type).HasColumnName("ar_type");
            this.Property(t => t.ar_report).HasColumnName("ar_report");
            this.Property(t => t.ar_reporteduser).HasColumnName("ar_reporteduser");
            this.Property(t => t.ar_targetid).HasColumnName("ar_targetid");
            this.Property(t => t.ar_reviewed).HasColumnName("ar_reviewed");
            this.Property(t => t.ar_datereported).HasColumnName("ar_datereported");
        }
    }
}
