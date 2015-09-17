using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.u_username);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.u_password)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.u_email)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.u_name)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.u_state)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.u_zip_code)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.u_city)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.u_signup_ip)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.u_lastsessionid)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.u_invited_by)
                .HasMaxLength(20);

            this.Property(t => t.u_deletereason)
                .HasMaxLength(200);

            this.Property(t => t.u_tokenuniqueid)
                .HasMaxLength(100);

            this.Property(t => t.u_profile_skin)
                .HasMaxLength(100);

            this.Property(t => t.u_status_text)
                .HasMaxLength(50);

            this.Property(t => t.u_myspaceid)
                .HasMaxLength(50);

            this.Property(t => t.u_country)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.u_password).HasColumnName("u_password");
            this.Property(t => t.u_email).HasColumnName("u_email");
            this.Property(t => t.u_name).HasColumnName("u_name");
            this.Property(t => t.u_gender).HasColumnName("u_gender");
            this.Property(t => t.u_birthdate).HasColumnName("u_birthdate");
            this.Property(t => t.u_birthdate2).HasColumnName("u_birthdate2");
            this.Property(t => t.u_usersince).HasColumnName("u_usersince");
            this.Property(t => t.u_active).HasColumnName("u_active");
            this.Property(t => t.u_smsconfirmed).HasColumnName("u_smsconfirmed");
            this.Property(t => t.u_prevlogin).HasColumnName("u_prevlogin");
            this.Property(t => t.u_lastlogin).HasColumnName("u_lastlogin");
            this.Property(t => t.u_logincount).HasColumnName("u_logincount");
            this.Property(t => t.u_lastonline).HasColumnName("u_lastonline");
            this.Property(t => t.u_profileviews).HasColumnName("u_profileviews");
            this.Property(t => t.u_profilevisible).HasColumnName("u_profilevisible");
            this.Property(t => t.u_receiveemails).HasColumnName("u_receiveemails");
            this.Property(t => t.u_state).HasColumnName("u_state");
            this.Property(t => t.u_zip_code).HasColumnName("u_zip_code");
            this.Property(t => t.u_city).HasColumnName("u_city");
            this.Property(t => t.u_deleted).HasColumnName("u_deleted");
            this.Property(t => t.u_paid_member).HasColumnName("u_paid_member");
            this.Property(t => t.u_signup_ip).HasColumnName("u_signup_ip");
            this.Property(t => t.u_interested_in).HasColumnName("u_interested_in");
            this.Property(t => t.u_lastsessionid).HasColumnName("u_lastsessionid");
            this.Property(t => t.u_message_verifications_left).HasColumnName("u_message_verifications_left");
            this.Property(t => t.u_verifiedbyadmin).HasColumnName("u_verifiedbyadmin");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.u_incoming_messages_restrictions).HasColumnName("u_incoming_messages_restrictions");
            this.Property(t => t.u_billingdetails).HasColumnName("u_billingdetails");
            this.Property(t => t.u_invited_by).HasColumnName("u_invited_by");
            this.Property(t => t.a_id).HasColumnName("a_id");
            this.Property(t => t.u_options).HasColumnName("u_options");
            this.Property(t => t.u_deletereason).HasColumnName("u_deletereason");
            this.Property(t => t.u_longitude).HasColumnName("u_longitude");
            this.Property(t => t.u_latitude).HasColumnName("u_latitude");
            this.Property(t => t.u_score).HasColumnName("u_score");
            this.Property(t => t.u_tokenuniqueid).HasColumnName("u_tokenuniqueid");
            this.Property(t => t.u_personalizationinfo).HasColumnName("u_personalizationinfo");
            this.Property(t => t.u_credits).HasColumnName("u_credits");
            this.Property(t => t.u_moderationscore).HasColumnName("u_moderationscore");
            this.Property(t => t.u_spamsuspected).HasColumnName("u_spamsuspected");
            this.Property(t => t.u_face_control_approved).HasColumnName("u_face_control_approved");
            this.Property(t => t.u_profile_skin).HasColumnName("u_profile_skin");
            this.Property(t => t.u_status_text).HasColumnName("u_status_text");
            this.Property(t => t.u_featuredmember).HasColumnName("u_featuredmember");
            this.Property(t => t.u_myspaceid).HasColumnName("u_myspaceid");
            this.Property(t => t.u_facebookid).HasColumnName("u_facebookid");
            this.Property(t => t.u_eventssettings).HasColumnName("u_eventssettings");
            this.Property(t => t.u_country).HasColumnName("u_country");
            this.Property(t => t.u_invitedby_count).HasColumnName("u_invitedby_count");
        }
    }
}
