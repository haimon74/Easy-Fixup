using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatAdminMap : EntityTypeConfiguration<ChatAdmin>
    {
        public ChatAdminMap()
        {
            // Primary Key
            this.HasKey(t => t.ca_username);

            // Properties
            this.Property(t => t.ca_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ca_password)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.ca_lastsessionid)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("ChatAdmins");
            this.Property(t => t.ca_username).HasColumnName("ca_username");
            this.Property(t => t.ca_password).HasColumnName("ca_password");
            this.Property(t => t.ca_lastlogin).HasColumnName("ca_lastlogin");
            this.Property(t => t.ca_lastsessionid).HasColumnName("ca_lastsessionid");
        }
    }
}
