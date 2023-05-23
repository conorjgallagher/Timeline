using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Appointment
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service) 
            : base(service)
        {
            LinkBuilders.Add(new AccountAppointmentsByContacts(service, this));
            LinkBuilders.Add(new AccountAppointmentsByPartyList(service, this));
            LinkBuilders.Add(new ContactAppointmentsByPartyList(service, this));
            LinkBuilders.Add(new OpportunityAppointmentsByRegarding(service, this));
        }

        protected override void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("appointment");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Appointment";
            Target["xrmc_entitylogicalname"] = "appointment";
            Target["xrmc_headlinefield"] = "subject";
            Target["xrmc_datefield"] = "actualend";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_4201.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='location' class='xRMC-Attribute'><span>Location:</span> </p>" + Environment.NewLine +
                "    <p id='organizer' class='xRMC-Attribute xRMC-PartyList'><span>Organizer:</span> </p>" + Environment.NewLine +
                "    <p id='requiredattendees' class='xRMC-Attribute xRMC-PartyList'><span>Required:</span> </p>" + Environment.NewLine +
                "    <p id='optionalattendees' class='xRMC-Attribute xRMC-PartyList'><span>Optional:</span> </p>" + Environment.NewLine +
                "    <p id='regardingobjectid' class='xRMC-Attribute xRMC-Reference'><span>Regarding:</span> </p>" + Environment.NewLine +
                "</div>" + Environment.NewLine +
                "<p>" + Environment.NewLine +
                "    <span>Description:</span>" + Environment.NewLine +
                "</p>" + Environment.NewLine +
                "<div class='innerwrapper'>" + Environment.NewLine +
                "    <p id='description' class='xRMC-Attribute'></p>" + Environment.NewLine +
                "</div>";
            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='appointment'>" + Environment.NewLine +
                "    <attribute name='subject' />" + Environment.NewLine +
                "    <attribute name='statecode' />" + Environment.NewLine +
                "    <attribute name='scheduledstart' />" + Environment.NewLine +
                "    <attribute name='scheduledend' />" + Environment.NewLine +
                "    <attribute name='actualstart' />" + Environment.NewLine +
                "    <attribute name='actualend' />" + Environment.NewLine +
                "    <attribute name='regardingobjectid' />" + Environment.NewLine +
                "    <attribute name='activityid' />" + Environment.NewLine +
                "    <attribute name='instancetypecode' />" + Environment.NewLine +
                "    <attribute name='requiredattendees' />" + Environment.NewLine +
                "    <attribute name='organizer' />" + Environment.NewLine +
                "    <attribute name='optionalattendees' />" + Environment.NewLine +
                "    <attribute name='location' />" + Environment.NewLine +
                "    <attribute name='description' />" + Environment.NewLine +
                "    <attribute name='createdon' />" + Environment.NewLine +
                "    <order attribute='actualend' descending='false' />" + Environment.NewLine +
                "    <order attribute='scheduledend' descending='false' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='activityid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";
            Target.Id = OrganizationService.Create(Target);
        }
    }
}