using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CreditsHistoryMap : EntityTypeConfiguration<CreditsHistory>
    {
        public CreditsHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ch_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CreditsHistory");
            this.Property(t => t.ch_id).HasColumnName("ch_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ch_date).HasColumnName("ch_date");
            this.Property(t => t.ch_service).HasColumnName("ch_service");
            this.Property(t => t.ch_amount).HasColumnName("ch_amount");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.CreditsHistories)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
