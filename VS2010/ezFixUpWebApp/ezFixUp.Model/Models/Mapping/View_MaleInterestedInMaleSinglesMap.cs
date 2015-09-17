using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class View_MaleInterestedInMaleSinglesMap : EntityTypeConfiguration<View_MaleInterestedInMaleSingles>
    {
        public View_MaleInterestedInMaleSinglesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.u_name, t.u_gender, t.u_birthdate, t.u_usersince, t.u_state, t.u_city, t.u_lastonline, t.u_score, t.u_credits, t.u_profileviews, t.u_paid_member, t.u_email, t.u_signup_ip });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.u_name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.u_gender)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_state)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.u_city)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.u_score)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_credits)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_status_text)
                .HasMaxLength(50);

            this.Property(t => t.u_country)
                .HasMaxLength(20);

            this.Property(t => t.u_profileviews)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.u_myspaceid)
                .HasMaxLength(50);

            this.Property(t => t.u_email)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.p_name)
                .HasMaxLength(80);

            this.Property(t => t.u_signup_ip)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("View_MaleInterestedInMaleSingles");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.u_name).HasColumnName("u_name");
            this.Property(t => t.u_gender).HasColumnName("u_gender");
            this.Property(t => t.u_birthdate).HasColumnName("u_birthdate");
            this.Property(t => t.u_usersince).HasColumnName("u_usersince");
            this.Property(t => t.u_state).HasColumnName("u_state");
            this.Property(t => t.u_city).HasColumnName("u_city");
            this.Property(t => t.u_lastonline).HasColumnName("u_lastonline");
            this.Property(t => t.u_interested_in).HasColumnName("u_interested_in");
            this.Property(t => t.u_score).HasColumnName("u_score");
            this.Property(t => t.u_credits).HasColumnName("u_credits");
            this.Property(t => t.u_status_text).HasColumnName("u_status_text");
            this.Property(t => t.u_facebookid).HasColumnName("u_facebookid");
            this.Property(t => t.u_country).HasColumnName("u_country");
            this.Property(t => t.u_profileviews).HasColumnName("u_profileviews");
            this.Property(t => t.u_myspaceid).HasColumnName("u_myspaceid");
            this.Property(t => t.u_paid_member).HasColumnName("u_paid_member");
            this.Property(t => t.u_email).HasColumnName("u_email");
            this.Property(t => t.p_primary).HasColumnName("p_primary");
            this.Property(t => t.p_approved).HasColumnName("p_approved");
            this.Property(t => t.p_private).HasColumnName("p_private");
            this.Property(t => t.p_explicit).HasColumnName("p_explicit");
            this.Property(t => t.p_image).HasColumnName("p_image");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.p_name).HasColumnName("p_name");
            this.Property(t => t.u_signup_ip).HasColumnName("u_signup_ip");
        }
    }
}
