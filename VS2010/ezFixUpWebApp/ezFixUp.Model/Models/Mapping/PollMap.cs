using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PollMap : EntityTypeConfiguration<Poll>
    {
        public PollMap()
        {
            // Primary Key
            this.HasKey(t => t.p_id);

            // Properties
            this.Property(t => t.p_title)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Polls");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.p_title).HasColumnName("p_title");
            this.Property(t => t.p_startdate).HasColumnName("p_startdate");
            this.Property(t => t.p_enddate).HasColumnName("p_enddate");
            this.Property(t => t.p_showresultsuntil).HasColumnName("p_showresultsuntil");
        }
    }
}
