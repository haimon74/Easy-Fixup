using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class TipsPopupMap : EntityTypeConfiguration<TipsPopup>
    {
        public TipsPopupMap()
        {
            // Primary Key
            this.HasKey(t => t.tp_id);

            // Properties
            this.Property(t => t.tp_page_name)
                .HasMaxLength(50);

            this.Property(t => t.tp_content)
                .HasMaxLength(2000);

            this.Property(t => t.tp_title)
                .HasMaxLength(250);

            this.Property(t => t.tp_name_key)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("TipsPopups");
            this.Property(t => t.tp_id).HasColumnName("tp_id");
            this.Property(t => t.tp_language_id).HasColumnName("tp_language_id");
            this.Property(t => t.tp_show_times).HasColumnName("tp_show_times");
            this.Property(t => t.tp_login_count_trigger).HasColumnName("tp_login_count_trigger");
            this.Property(t => t.tp_days_count_trigger).HasColumnName("tp_days_count_trigger");
            this.Property(t => t.tp_page_name).HasColumnName("tp_page_name");
            this.Property(t => t.tp_min_credits_trigger).HasColumnName("tp_min_credits_trigger");
            this.Property(t => t.tp_no_spending_days_trigger).HasColumnName("tp_no_spending_days_trigger");
            this.Property(t => t.tp_content).HasColumnName("tp_content");
            this.Property(t => t.tp_title).HasColumnName("tp_title");
            this.Property(t => t.tp_name_key).HasColumnName("tp_name_key");
            this.Property(t => t.tp_matchmaker_view).HasColumnName("tp_matchmaker_view");
        }
    }
}
