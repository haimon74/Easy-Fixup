using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EstablishedCommunicationMap : EntityTypeConfiguration<EstablishedCommunication>
    {
        public EstablishedCommunicationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ec_from_username, t.ec_to_username });

            // Properties
            this.Property(t => t.ec_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ec_to_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("EstablishedCommunications");
            this.Property(t => t.ec_from_username).HasColumnName("ec_from_username");
            this.Property(t => t.ec_to_username).HasColumnName("ec_to_username");
            this.Property(t => t.ec_date).HasColumnName("ec_date");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.EstablishedCommunications)
                .HasForeignKey(d => d.ec_from_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.EstablishedCommunications1)
                .HasForeignKey(d => d.ec_to_username).WillCascadeOnDelete(false);

        }
    }
}
