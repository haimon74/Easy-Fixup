using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class MatchmakingMap : EntityTypeConfiguration<Matchmaking>
    {
        public MatchmakingMap()
        {
            // Primary Key
            this.HasKey(t => t.mm_id);

            // Properties
            this.Property(t => t.mm_matchmaker_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.mm_to_friend_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.mm_with_friend_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.mm_status)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Matchmakings");
            this.Property(t => t.mm_id).HasColumnName("mm_id");
            this.Property(t => t.mm_matchmaker_username).HasColumnName("mm_matchmaker_username");
            this.Property(t => t.mm_to_friend_username).HasColumnName("mm_to_friend_username");
            this.Property(t => t.mm_with_friend_username).HasColumnName("mm_with_friend_username");
            this.Property(t => t.mm_timestamp).HasColumnName("mm_timestamp");
            this.Property(t => t.mm_to_friend_ack).HasColumnName("mm_to_friend_ack");
            this.Property(t => t.mm_with_friend_ack).HasColumnName("mm_with_friend_ack");
            this.Property(t => t.mm_status).HasColumnName("mm_status");
        }
    }
}
