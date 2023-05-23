using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
namespace Timeline.Plugins.EntityConfiguration
{
    public class TimelineEntityFactory
    {
        public static List<IBuilder> CreateDefaultBuilders(IOrganizationService service)
        {
            var builders = new List<IBuilder>
                               {
                                   new Appointment.ConfigurationBuilder(service),
                                   new CampaignResponse.ConfigurationBuilder(service),
                                   new Case.ConfigurationBuilder(service),
                                   new Email.ConfigurationBuilder(service),
                                   new Fax.ConfigurationBuilder(service),
                                   new Letter.ConfigurationBuilder(service),
                                   new Opportunity.ConfigurationBuilder(service),
                                   new Order.ConfigurationBuilder(service),
                                   new Phonecall.ConfigurationBuilder(service),
                                   new ServiceAppointment.ConfigurationBuilder(service),
                                   new Task.ConfigurationBuilder(service)
                               };
            return builders;
        }

        public static Entity CreateTimelineEntity()
        {
            var timelineEntity = new Entity("xrmc_timelineentity");
            timelineEntity["xrmc_maxrecords"] = 5;
            timelineEntity["xrmc_daylimit"] = 100;
            return timelineEntity;
        }
    }
}