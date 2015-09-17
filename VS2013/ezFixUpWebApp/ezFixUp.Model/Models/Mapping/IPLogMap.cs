using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class IPLogMap : EntityTypeConfiguration<IPLog>
    {
        public IPLogMap()
        {
            // Primary Key
            this.HasKey(t => t.ipl_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ipl_ip)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("IPLogs");
            this.Property(t => t.ipl_id).HasColumnName("ipl_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ipl_ip).HasColumnName("ipl_ip");
            this.Property(t => t.ipl_action).HasColumnName("ipl_action");
            this.Property(t => t.ipl_timestamp).HasColumnName("ipl_timestamp");
        }
    }
}
