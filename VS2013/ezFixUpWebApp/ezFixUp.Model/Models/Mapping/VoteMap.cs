using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class VoteMap : EntityTypeConfiguration<Vote>
    {
        public VoteMap()
        {
            // Primary Key
            this.HasKey(t => new { t.v_fromusername, t.v_tousername });

            // Properties
            this.Property(t => t.v_fromusername)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.v_tousername)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Votes");
            this.Property(t => t.v_fromusername).HasColumnName("v_fromusername");
            this.Property(t => t.v_tousername).HasColumnName("v_tousername");
            this.Property(t => t.v_score).HasColumnName("v_score");
            this.Property(t => t.v_timestamp).HasColumnName("v_timestamp");
        }
    }
}
