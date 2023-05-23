using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Timeline.Plugins.EntityConfiguration
{
    public class BaseLinkBuilder
    {
        protected readonly IOrganizationService service;
        protected readonly IBuilder parent;
        public Entity Target { get; set; }

        protected BaseLinkBuilder(IOrganizationService service, IBuilder parent)
        {
            this.service = service;
            this.parent = parent;
        }

        protected Entity RetrieveExistingConfig(string name)
        {
            using (var ctx = new OrganizationServiceContext(service))
            {
                Entity config = (from e in ctx.CreateQuery("xrmc_timelineentitylink")
                                 where (string)e["xrmc_name"] == name
                                 select e).FirstOrDefault();
                return config;
            }
        }
    }
}