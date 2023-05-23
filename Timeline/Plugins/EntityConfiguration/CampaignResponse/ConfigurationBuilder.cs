using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.CampaignResponse
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service) 
            : base(service)
        {
            LinkBuilders.Add(new AccountCampaignResponsesByPartyList(service, this));
            LinkBuilders.Add(new AccountCampaignResponsesByContacts(service, this));
            LinkBuilders.Add(new ContactCampaignResponsesByPartyList(service, this));
            LinkBuilders.Add(new OpportunityCampaignResponsesByRegarding(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("campaignresponse");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Campaign Response";
            Target["xrmc_entitylogicalname"] = "campaignresponse";
            Target["xrmc_headlinefield"] = "subject";
            Target["xrmc_datefield"] = "actualend";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_4401.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='regardingobjectid' class='xRMC-Attribute xRMC-Reference'><span>Parent Campaign:</span> </p>" + Environment.NewLine +
                "    <p id='responsecode' class='xRMC-Attribute xRMC-FormattedValue'><span>Response Code:</span> </p>" + Environment.NewLine +
                "    <p id='promotioncodename' class='xRMC-Attribute'><span>Promotion Code:</span> </p>" + Environment.NewLine +
                "    <p id='customer' class='xRMC-Attribute xRMC-PartyList'><span>Customer:</span> </p>" + Environment.NewLine +
                "    <p><span>Description:</span></p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='campaignresponse'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='activityid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);
        }
    }
}
