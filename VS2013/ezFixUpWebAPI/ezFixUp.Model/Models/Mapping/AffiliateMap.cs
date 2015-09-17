using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AffiliateMap : EntityTypeConfiguration<Affiliate>
    {
        public AffiliateMap()
        {
            // Primary Key
            this.HasKey(t => t.a_id);

            // Properties
            this.Property(t => t.a_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.a_password)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.a_name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.a_email)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.a_siteurl)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.a_payment_details)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Affiliates");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.a_username).HasColumnName("a_username");
            this.Property(t => t.a_password).HasColumnName("a_password");
            this.Property(t => t.a_name).HasColumnName("a_name");
            this.Property(t => t.a_email).HasColumnName("a_email");
            this.Property(t => t.a_siteurl).HasColumnName("a_siteurl");
            this.Property(t => t.a_payment_details).HasColumnName("a_payment_details");
            this.Property(t => t.a_deleted).HasColumnName("a_deleted");
            this.Property(t => t.a_active).HasColumnName("a_active");
            this.Property(t => t.a_percentage).HasColumnName("a_percentage");
            this.Property(t => t.a_fixed_amount).HasColumnName("a_fixed_amount");
            this.Property(t => t.a_recurrent).HasColumnName("a_recurrent");
            this.Property(t => t.a_balance).HasColumnName("a_balance");
            this.Property(t => t.a_request_payment).HasColumnName("a_request_payment");
        }
    }
}
