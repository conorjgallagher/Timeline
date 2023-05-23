using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Fax
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service) 
            : base(service)
        {
            LinkBuilders.Add(new AccountFaxesByPartyList(service, this));
            LinkBuilders.Add(new AccountFaxesByContact(service, this));
            LinkBuilders.Add(new ContactFaxesByPartyList(service, this));
            LinkBuilders.Add(new OpportunityFaxesByRegarding(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("fax");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Fax";
            Target["xrmc_entitylogicalname"] = "fax";
            Target["xrmc_headlinefield"] = "subject";
            Target["xrmc_datefield"] = "actualend";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_4204.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='to' class='xRMC-Attribute xRMC-PartyList'><span>Recipient:</span> </p>" + Environment.NewLine +
                "    <p id='from' class='xRMC-Attribute xRMC-PartyList'><span>From:</span> </p>" + Environment.NewLine +
                "    <p id='faxnumber' class='xRMC-Attribute'><span>Fax Number:</span> </p>" + Environment.NewLine +
                "    <p id='regardingobjectid' class='xRMC-Attribute xRMC-Reference'><span>Regarding:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p>" + Environment.NewLine +
                "    <span>Description:</span>" + Environment.NewLine +
                "</p>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute></p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='fax'>" + Environment.NewLine +
                "    <attribute name='subject' />" + Environment.NewLine +
                "    <attribute name='regardingobjectid' />" + Environment.NewLine +
                "    <attribute name='activityid' />" + Environment.NewLine +
                "    <attribute name='to' />" + Environment.NewLine +
                "    <attribute name='from' />" + Environment.NewLine +
                "    <attribute name='faxnumber' />" + Environment.NewLine +
                "    <attribute name='description' />" + Environment.NewLine +
                "    <order attribute='subject' descending='false' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='activityid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);

        }
    }
}
