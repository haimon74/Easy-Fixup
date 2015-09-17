using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AdminMap : EntityTypeConfiguration<Admin>
    {
        public AdminMap()
        {
            // Primary Key
            this.HasKey(t => t.a_username);

            // Properties
            this.Property(t => t.a_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.a_password)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.a_privileges)
                .IsRequired();

            this.Property(t => t.a_lastsessionid)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("Admins");
            this.Property(t => t.a_username).HasColumnName("a_username");
            this.Property(t => t.a_password).HasColumnName("a_password");
            this.Property(t => t.a_lastlogin).HasColumnName("a_lastlogin");
            this.Property(t => t.a_privileges).HasColumnName("a_privileges");
            this.Property(t => t.a_lastsessionid).HasColumnName("a_lastsessionid");
        }
    }
}
