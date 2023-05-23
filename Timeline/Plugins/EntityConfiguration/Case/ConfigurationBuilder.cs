using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Case
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service) 
            : base(service)
        {
            LinkBuilders.Add(new AccountCasesByCustomer(service, this));
            LinkBuilders.Add(new AccountCasesByContact(service, this));
            LinkBuilders.Add(new ContactCasesByCustomer(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("incident");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Cases";
            Target["xrmc_entitylogicalname"] = "incident";
            Target["xrmc_headlinefield"] = "title";
            Target["xrmc_datefield"] = "createdon";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_112.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='ticketnumber' class='xRMC-Attribute'><span>ID:</span> </p>" + Environment.NewLine +
                "    <p id='title' class='xRMC-Attribute'><span>Subject:</span> </p>" + Environment.NewLine +
                "    <p id='prioritycode' class='xRMC-Attribute xRMC-FormattedValue'><span>Priority:</span> </p>" + Environment.NewLine +
                "    <p id='customerid' class='xRMC-Attribute xRMC-Reference'><span>Customer:</span> </p>" + Environment.NewLine +
                "    <p id='casetypecode' class='xRMC-Attribute xRMC-FormattedValue'><span>Type:</span> </p>" + Environment.NewLine +
                "    <p id='caseorigincode' class='xRMC-Attribute xRMC-FormattedValue'><span>Revenue:</span> </p>" + Environment.NewLine +
                "    <p id='followupby' class='xRMC-Attribute xRMC-Date'><span>Follow Up By:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p><span>Description:</span></p>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='incident'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='incidentid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);
        }
    }
}
