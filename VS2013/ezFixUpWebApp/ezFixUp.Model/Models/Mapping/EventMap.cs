using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.e_id);

            // Properties
            this.Property(t => t.e_fromusername)
                .HasMaxLength(20);

            this.Property(t => t.e_details)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("Events");
            this.Property(t => t.e_id).HasColumnName("e_id");
            this.Property(t => t.e_fromusername).HasColumnName("e_fromusername");
            this.Property(t => t.e_fromgroup).HasColumnName("e_fromgroup");
            this.Property(t => t.e_type).HasColumnName("e_type");
            this.Property(t => t.e_details).HasColumnName("e_details");
            this.Property(t => t.e_date).HasColumnName("e_date");

            // Relationships
            this.HasOptional(t => t.Group)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.e_fromgroup).WillCascadeOnDelete(false);
            this.HasOptional(t => t.User)
                .WithMany(t => t.Events)
                .HasForeignKey(d => d.e_fromusername).WillCascadeOnDelete(false);

        }
    }
}
