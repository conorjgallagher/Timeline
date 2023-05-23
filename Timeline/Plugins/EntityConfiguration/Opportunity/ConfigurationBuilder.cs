using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Opportunity
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service)
            : base(service)
        {
            LinkBuilders.Add(new AccountOpportunitiesByCustomer(service, this));
            LinkBuilders.Add(new AccountOpportunitiesByContact(service, this));
            LinkBuilders.Add(new ContactOpportunitiesByCustomer(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("opportunity");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Opportunity";
            Target["xrmc_entitylogicalname"] = "opportunity";
            Target["xrmc_headlinefield"] = "name";
            Target["xrmc_datefield"] = "actualclosedate";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_3.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='date' class='xRMC-Attribute xRMC-OpportunityDate'><span>Close Date:</span> </p>" + Environment.NewLine +
                "    <p id='revenue' class='xRMC-Attribute xRMC-OpportunityValue'><span>Revenue:</span> </p>" + Environment.NewLine +
                "    <p id='ownerid' class='xRMC-Attribute xRMC-Reference'><span>Owner:</span> </p>" + Environment.NewLine +
                "    <p id='customerid' class='xRMC-Attribute xRMC-Reference'><span>Customer:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p><span>Description:</span></p>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<div for='currentsituation' class='xRMC-AutoHide'>" + Environment.NewLine +
                "    <p><span>Current Situation:</span></p>" + Environment.NewLine +
                "    <div class='innerwrapper' id='currentsituation'>" + Environment.NewLine +
                "        <p id='currentsituation' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "    </div>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<div for='customerneed' class='xRMC-AutoHide'>" + Environment.NewLine +
                "    <p><span>Customer Need:</span></p>" + Environment.NewLine +
                "    <div class='innerwrapper' id='customerneed'>" + Environment.NewLine +
                "        <p id='customerneed' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "    </div>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<div for='proposedsolution' class='xRMC-AutoHide'>" + Environment.NewLine +
                "    <p><span>Proposed Solution:</span></p>" + Environment.NewLine +
                "    <div class='innerwrapper' id='proposedsolution'>" + Environment.NewLine +
                "        <p id='proposedsolution' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "    </div>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "    <entity name='opportunity'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "      <filter type='and'>" + Environment.NewLine +
                "        <condition attribute='opportunityid' operator='eq' value='{0}' />" + Environment.NewLine +
                "      </filter>" + Environment.NewLine +
                "    </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);
        }
    }
}
