using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EmailQueueMap : EntityTypeConfiguration<EmailQueue>
    {
        public EmailQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.eq_id);

            // Properties
            this.Property(t => t.eq_from)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.eq_to)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.eq_fromname)
                .HasMaxLength(250);

            this.Property(t => t.eq_toname)
                .HasMaxLength(250);

            this.Property(t => t.eq_cc)
                .HasMaxLength(250);

            this.Property(t => t.eq_bcc)
                .HasMaxLength(250);

            this.Property(t => t.eq_subject)
                .IsRequired();

            this.Property(t => t.eq_body)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("EmailQueue");
            this.Property(t => t.eq_id).HasColumnName("eq_id");
            this.Property(t => t.eq_from).HasColumnName("eq_from");
            this.Property(t => t.eq_to).HasColumnName("eq_to");
            this.Property(t => t.eq_fromname).HasColumnName("eq_fromname");
            this.Property(t => t.eq_toname).HasColumnName("eq_toname");
            this.Property(t => t.eq_cc).HasColumnName("eq_cc");
            this.Property(t => t.eq_bcc).HasColumnName("eq_bcc");
            this.Property(t => t.eq_subject).HasColumnName("eq_subject");
            this.Property(t => t.eq_body).HasColumnName("eq_body");
            this.Property(t => t.eq_tries).HasColumnName("eq_tries");
            this.Property(t => t.eq_last_try).HasColumnName("eq_last_try");
            this.Property(t => t.eq_last_error).HasColumnName("eq_last_error");
            this.Property(t => t.eq_next_try).HasColumnName("eq_next_try");
        }
    }
}
