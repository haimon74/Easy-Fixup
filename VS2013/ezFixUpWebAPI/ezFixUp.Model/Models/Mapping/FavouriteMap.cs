using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FavouriteMap : EntityTypeConfiguration<Favourite>
    {
        public FavouriteMap()
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
            this.ToTable("Favourites");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.f_username).HasColumnName("f_username");
            this.Property(t => t.f_timestamp).HasColumnName("f_timestamp");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Favourites)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Favourites1)
                .HasForeignKey(d => d.f_username).WillCascadeOnDelete(false);

        }
    }
}
