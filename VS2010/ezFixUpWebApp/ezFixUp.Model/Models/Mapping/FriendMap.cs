using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FriendMap : EntityTypeConfiguration<Friend>
    {
        public FriendMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.f_username });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.f_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Friends");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.f_username).HasColumnName("f_username");
            this.Property(t => t.f_timestamp).HasColumnName("f_timestamp");
            this.Property(t => t.f_accepted).HasColumnName("f_accepted");
            this.Property(t => t.fb_mutualy_friends).HasColumnName("fb_mutualy_friends");
            this.Property(t => t.f_is_facebook_friend).HasColumnName("f_is_facebook_friend");
            this.Property(t => t.f_denied_counter).HasColumnName("f_denied_counter");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Friends)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Friends1)
                .HasForeignKey(d => d.f_username).WillCascadeOnDelete(false);

        }
    }
}
