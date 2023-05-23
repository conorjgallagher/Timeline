using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Timeline.Plugins.EntityConfiguration
{
    public abstract class BaseEntityBuilder : IBuilder
    {
        protected readonly IOrganizationService OrganizationService;
        protected readonly List<IBuilder> LinkBuilders;
        public Entity Target { get; set; }

        protected BaseEntityBuilder(IOrganizationService service)
        {
            OrganizationService = service;
            LinkBuilders = new List<IBuilder>();
        }

        public void Construct()
        {
            CreateMainConfiguration();
            CreateEntityLinks();
        }

        abstract protected void CreateMainConfiguration();

        protected void CreateEntityLinks()
        {
            if (LinkBuilders != null)
            {
                LinkBuilders.ForEach(e => e.Construct());
            }
        }

        protected Entity RetrieveExistingConfig(string entityTypeName)
        {
            using (var ctx = new OrganizationServiceContext(OrganizationService))
            {
                Entity config = (from e in ctx.CreateQuery("xrmc_timelineentity")
                                 where (string)e["xrmc_entitylogicalname"] == entityTypeName
                                 select e).FirstOrDefault();
                return config;
            }

        }
    }

}
