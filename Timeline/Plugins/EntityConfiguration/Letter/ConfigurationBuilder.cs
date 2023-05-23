using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Letter
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service)
            : base(service)
        {
            LinkBuilders.Add(new AccountLettersByPartyList(service, this));
            LinkBuilders.Add(new AccountLettersByContact(service, this));
            LinkBuilders.Add(new ContactLettersByPartyList(service, this));
            LinkBuilders.Add(new OpportunityLettersByRegarding(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("letter");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Letter";
            Target["xrmc_entitylogicalname"] = "letter";
            Target["xrmc_headlinefield"] = "subject";
            Target["xrmc_datefield"] = "actualend";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_4207.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='from' class='xRMC-Attribute xRMC-PartyList'><span>Sender:</span> </p>" + Environment.NewLine +
                "    <p id='to' class='xRMC-Attribute xRMC-PartyList'><span>Recipient:</span> </p>" + Environment.NewLine +
                "    <p id='address' class='xRMC-Attribute'><span>Address:</span> </p>" + Environment.NewLine +
                "    <p id='regardingobjectid' class='xRMC-Attribute xRMC-Reference'><span>Regarding:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p>" + Environment.NewLine +
                "    <span>Description:</span>" + Environment.NewLine +
                "</p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='letter'>" + Environment.NewLine +
                "    <attribute name='subject' />" + Environment.NewLine +
                "    <attribute name='statecode' />" + Environment.NewLine +
                "    <attribute name='prioritycode' />" + Environment.NewLine +
                "    <attribute name='scheduledend' />" + Environment.NewLine +
                "    <attribute name='regardingobjectid' />" + Environment.NewLine +
                "    <attribute name='activityid' />" + Environment.NewLine +
                "    <attribute name='subcategory' />" + Environment.NewLine +
                "    <attribute name='statuscode' />" + Environment.NewLine +
                "    <attribute name='scheduledstart' />" + Environment.NewLine +
                "    <attribute name='from' />" + Environment.NewLine +
                "    <attribute name='to' />" + Environment.NewLine +
                "    <attribute name='ownerid' />" + Environment.NewLine +
                "    <attribute name='description' />" + Environment.NewLine +
                "    <attribute name='createdon' />" + Environment.NewLine +
                "    <attribute name='category' />" + Environment.NewLine +
                "    <attribute name='address' />" + Environment.NewLine +
                "    <attribute name='actualstart' />" + Environment.NewLine +
                "    <attribute name='actualend' />" + Environment.NewLine +
                "    <attribute name='activitytypecode' />" + Environment.NewLine +
                "    <order attribute='scheduledend' descending='true' />" + Environment.NewLine +
                "    <order attribute='actualend' descending='true' />" + Environment.NewLine +
                "    <order attribute='actualstart' descending='true' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='activityid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";

            Target.Id = OrganizationService.Create(Target);

        }
    }
}
