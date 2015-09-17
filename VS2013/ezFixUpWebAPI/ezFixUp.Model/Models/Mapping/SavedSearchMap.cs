using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class SavedSearchMap : EntityTypeConfiguration<SavedSearch>
    {
        public SavedSearchMap()
        {
            // Primary Key
            this.HasKey(t => t.ss_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ss_name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ss_country)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ss_state)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ss_zip)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ss_city)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ss_choiceids)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SavedSearches");
            this.Property(t => t.ss_id).HasColumnName("ss_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ss_name).HasColumnName("ss_name");
            this.Property(t => t.ss_gender).HasColumnName("ss_gender");
            this.Property(t => t.ss_country).HasColumnName("ss_country");
            this.Property(t => t.ss_state).HasColumnName("ss_state");
            this.Property(t => t.ss_zip).HasColumnName("ss_zip");
            this.Property(t => t.ss_city).HasColumnName("ss_city");
            this.Property(t => t.ss_agefrom).HasColumnName("ss_agefrom");
            this.Property(t => t.ss_ageto).HasColumnName("ss_ageto");
            this.Property(t => t.ss_photoreq).HasColumnName("ss_photoreq");
            this.Property(t => t.ss_choiceids).HasColumnName("ss_choiceids");
            this.Property(t => t.ss_emailmatches).HasColumnName("ss_emailmatches");
            this.Property(t => t.ss_nextemaildate).HasColumnName("ss_nextemaildate");
            this.Property(t => t.ss_emailfrequency).HasColumnName("ss_emailfrequency");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.SavedSearches)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
