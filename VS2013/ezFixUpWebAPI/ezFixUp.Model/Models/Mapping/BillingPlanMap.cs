using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class BillingPlanMap : EntityTypeConfiguration<BillingPlan>
    {
        public BillingPlanMap()
        {
            // Primary Key
            this.HasKey(t => t.p_id);

            // Properties
            this.Property(t => t.p_title)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("BillingPlans");
            this.Property(t => t.p_id).HasColumnName("p_id");
            this.Property(t => t.p_title).HasColumnName("p_title");
            this.Property(t => t.p_amount).HasColumnName("p_amount");
            this.Property(t => t.p_billing_cycle).HasColumnName("p_billing_cycle");
            this.Property(t => t.p_billing_cycle_unit).HasColumnName("p_billing_cycle_unit");
            this.Property(t => t.p_deleted).HasColumnName("p_deleted");
            this.Property(t => t.p_options).HasColumnName("p_options");
        }
    }
}
