using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ContactUMap : EntityTypeConfiguration<ContactU>
    {
        public ContactUMap()
        {
            // Primary Key
            this.HasKey(t => t.cu_id);

            // Properties
            this.Property(t => t.cu_reportedby)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.cu_report)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.cu_reporteduser)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ContactUs");
            this.Property(t => t.cu_id).HasColumnName("cu_id");
            this.Property(t => t.cu_reportedby).HasColumnName("cu_reportedby");
            this.Property(t => t.cu_type).HasColumnName("cu_type");
            this.Property(t => t.cu_report).HasColumnName("cu_report");
            this.Property(t => t.cu_reporteduser).HasColumnName("cu_reporteduser");
            this.Property(t => t.cu_targetid).HasColumnName("cu_targetid");
            this.Property(t => t.cu_reviewed).HasColumnName("cu_reviewed");
            this.Property(t => t.cu_datereported).HasColumnName("cu_datereported");
        }
    }
}
