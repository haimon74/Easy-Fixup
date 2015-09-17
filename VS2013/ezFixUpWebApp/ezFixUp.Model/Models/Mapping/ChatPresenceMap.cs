using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatPresenceMap : EntityTypeConfiguration<ChatPresence>
    {
        public ChatPresenceMap()
        {
            // Primary Key
            this.HasKey(t => t.cp_username);

            // Properties
            this.Property(t => t.cp_username)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ChatPresence");
            this.Property(t => t.cp_username).HasColumnName("cp_username");
            this.Property(t => t.cp_lastonline).HasColumnName("cp_lastonline");
        }
    }
}
