using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class PhotoContestMap : EntityTypeConfiguration<PhotoContest>
    {
        public PhotoContestMap()
        {
            // Primary Key
            this.HasKey(t => t.pc_id);

            // Properties
            this.Property(t => t.pc_name)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.pc_description)
                .IsRequired();

            this.Property(t => t.pc_terms)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PhotoContests");
            this.Property(t => t.pc_id).HasColumnName("pc_id");
            this.Property(t => t.pc_name).HasColumnName("pc_name");
            this.Property(t => t.pc_gender).HasColumnName("pc_gender");
            this.Property(t => t.pc_description).HasColumnName("pc_description");
            this.Property(t => t.pc_terms).HasColumnName("pc_terms");
            this.Property(t => t.pc_minage).HasColumnName("pc_minage");
            this.Property(t => t.pc_maxage).HasColumnName("pc_maxage");
            this.Property(t => t.pc_datecreated).HasColumnName("pc_datecreated");
            this.Property(t => t.pc_dateends).HasColumnName("pc_dateends");
        }
    }
}
