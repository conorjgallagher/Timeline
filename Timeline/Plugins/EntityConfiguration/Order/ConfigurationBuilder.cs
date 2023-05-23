using System;
using Microsoft.Xrm.Sdk;
using Timeline.Plugins.EntityConfiguration.Opportunity;

namespace Timeline.Plugins.EntityConfiguration.Order
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service)
            : base(service)
        {
            LinkBuilders.Add(new AccountOrdersByCustomer(service, this));
            LinkBuilders.Add(new AccountOrdersByContact(service, this));
            LinkBuilders.Add(new ContactOrdersByCustomer(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("salesorder");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Order";
            Target["xrmc_entitylogicalname"] = "salesorder";
            Target["xrmc_headlinefield"] = "orderid";
            Target["xrmc_datefield"] = "submitdate";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_1088.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='ordernumber' class='xRMC-Attribute'><span class='widelabel'>Order ID:</span> </p>" + Environment.NewLine +
                "    <p id='pricelevelid' class='xRMC-Attribute xRMC-Reference'><span class='widelabel'>Price List:</span> </p>" + Environment.NewLine +
                "    <p id='totallineitemamount' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Detail Amount:</span> </p>" + Environment.NewLine +
                "    <p id='discountamount' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Order Discount:</span> </p>" + Environment.NewLine +
                "    <p id='totalamountlessfreight' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Pre-Freight Amount:</span> </p>" + Environment.NewLine +
                "    <p id='freightamount' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Freight Amount:</span> </p>" + Environment.NewLine +
                "    <p id='totaltax' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Total Tax:</span> </p>" + Environment.NewLine +
                "    <p id='totalamount' class='xRMC-Attribute xRMC-FormattedValue'><span class='widelabel'>Total Amount:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p><span>Description:</span></p>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "<p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='salesorder'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "    <order attribute='createdon' descending='true' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='salesorderid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);
        }
    }
}
