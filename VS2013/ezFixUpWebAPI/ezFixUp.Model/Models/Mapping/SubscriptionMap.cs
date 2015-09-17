using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class SubscriptionMap : EntityTypeConfiguration<Subscription>
    {
        public SubscriptionMap()
        {
            // Primary Key
            this.HasKey(t => t.s_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.s_paymentprocessor)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.s_custom)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Subscriptions");
            this.Property(t => t.s_id).HasColumnName("s_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.s_order_date).HasColumnName("s_order_date");
            this.Property(t => t.s_renew_date).HasColumnName("s_renew_date");
            this.Property(t => t.s_confirmed).HasColumnName("s_confirmed");
            this.Property(t => t.s_cancelled).HasColumnName("s_cancelled");
            this.Property(t => t.s_cancellation_requested).HasColumnName("s_cancellation_requested");
            this.Property(t => t.s_paymentprocessor).HasColumnName("s_paymentprocessor");
            this.Property(t => t.s_custom).HasColumnName("s_custom");

            // Relationships
            this.HasRequired(t => t.BillingPlan)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(d => d.p_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
