using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Order
{
    class ContactOrdersByCustomer : BaseLinkBuilder, IBuilder
    {
        public ContactOrdersByCustomer(IOrganizationService service, IBuilder parent)
            : base(service, parent)
        {
        }

        public void Construct()
        {
            // If a config already exists simply return it rather than creating a new one
            const string linkName = "Contact Orders by Customer";
            Target = RetrieveExistingConfig(linkName);
            if (Target != null) return;

            Target = new Entity("xrmc_timelineentitylink");
            Target["xrmc_name"] = linkName;
            Target["xrmc_logicalentityname"] = "contact";
            Target["xrmc_timelineentity"] = parent.Target.ToEntityReference();
            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='salesorder'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "    <order attribute='submitdate' descending='true' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "      <condition attribute='customerid' operator='eq' value='{0}' />" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";
            Target.Id = service.Create(Target);
        }
    }
}
