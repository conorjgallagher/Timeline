using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.ServiceAppointment
{
    public class ConfigurationBuilder : BaseEntityBuilder
    {
        public ConfigurationBuilder(IOrganizationService service)
            : base(service)
        {
            LinkBuilders.Add(new AccountServiceAppointmentsByPartyList(service, this));
            LinkBuilders.Add(new AccountServiceAppointmentsByContact(service, this));
            LinkBuilders.Add(new ContactServiceAppointmentsByPartyList(service, this));
            LinkBuilders.Add(new OpportunityServiceAppointmentsByRegarding(service, this));
        }

        override protected void CreateMainConfiguration()
        {
            // If a config already exists simply return it rather than creating a new one
            Target = RetrieveExistingConfig("serviceappointment");
            if (Target != null) return;

            // Create a new config
            Target = TimelineEntityFactory.CreateTimelineEntity();
            Target["xrmc_name"] = "Service Appointment";
            Target["xrmc_entitylogicalname"] = "serviceappointment";
            Target["xrmc_headlinefield"] = "subject";
            Target["xrmc_datefield"] = "actualend";
            Target["xrmc_smallicon"] = "/_imgs/ico_16_4214.gif";
            Target["xrmc_html"] =
                "<div class='summaryinfo'>" + Environment.NewLine +
                "    <div class='openentity'><a id='recordURL' href='#' title='Open full record'></a></div>" + Environment.NewLine +
                "    <p id='serviceid' class='xRMC-Attribute xRMC-Reference'><span>Service:</span> </p>" + Environment.NewLine +
                "    <p id='siteid' class='xRMC-Attribute xRMC-Reference'><span>Site:</span> </p>" + Environment.NewLine +
                "    <p id='customers' class='xRMC-Attribute xRMC-PartyList'><span>Customers:</span> </p>" + Environment.NewLine +
                "    <p id='resources' class='xRMC-Attribute xRMC-PartyList'><span>Resources:</span> </p>" + Environment.NewLine +
                "    <p id='location' class='xRMC-Attribute'><span>Location:</span> </p>" + Environment.NewLine +
                "    <p id='scheduleddurationminutes' class='xRMC-Attribute xRMC-FormattedValue'><span>Duration (mins):</span> </p>" + Environment.NewLine +
                "    <p id='regardingobjectid' class='xRMC-Attribute xRMC-Reference'><span>Regarding:</span> </p>" + Environment.NewLine +
                "</div>";

            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='serviceappointment'>" + Environment.NewLine +
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
