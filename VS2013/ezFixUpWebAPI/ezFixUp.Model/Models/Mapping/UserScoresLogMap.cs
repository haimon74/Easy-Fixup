using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class UserScoresLogMap : EntityTypeConfiguration<UserScoresLog>
    {
        public UserScoresLogMap()
        {
            // Primary Key
            this.HasKey(t => t.usl_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.usl_notes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("UserScoresLog");
            this.Property(t => t.usl_id).HasColumnName("usl_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.usl_timestamp).HasColumnName("usl_timestamp");
            this.Property(t => t.usl_score).HasColumnName("usl_score");
            this.Property(t => t.usl_notes).HasColumnName("usl_notes");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserScoresLogs)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
