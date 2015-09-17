using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class TwitterCredentialMap : EntityTypeConfiguration<TwitterCredential>
    {
        public TwitterCredentialMap()
        {
            // Primary Key
            this.HasKey(t => t.u_username);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.tc_username)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.tc_password)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("TwitterCredentials");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.tc_username).HasColumnName("tc_username");
            this.Property(t => t.tc_password).HasColumnName("tc_password");

            // Relationships
            this.HasRequired(t => t.User)
                .WithOptional(t => t.TwitterCredential).WillCascadeOnDelete(false);

        }
    }
}
