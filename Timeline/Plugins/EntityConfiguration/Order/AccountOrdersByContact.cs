using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Order
{
    public class AccountOrdersByContact : BaseLinkBuilder, IBuilder
    {
        public AccountOrdersByContact(IOrganizationService service, IBuilder parent)
            : base(service, parent)
        {
        }

        public void Construct()
        {
            // If a config already exists simply return it rather than creating a new one
            const string linkName = "Account Orders by Contact";
            Target = RetrieveExistingConfig(linkName);
            if (Target != null) return;

            Target = new Entity("xrmc_timelineentitylink");
            Target["xrmc_name"] = linkName;
            Target["xrmc_logicalentityname"] = "account";
            Target["xrmc_timelineentity"] = parent.Target.ToEntityReference();
            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='salesorder'>" + Environment.NewLine +
                "    <all-attributes />" + Environment.NewLine +
                "    <order attribute='submitdate' descending='true' />" + Environment.NewLine +
                "    <link-entity name='contact' alias='aa' to='customerid' from='contactid'>" + Environment.NewLine +
                "        <filter type='and'>" + Environment.NewLine +
                "            <condition attribute='parentcustomerid' value='{0}' operator='eq'/>" + Environment.NewLine +
                "        </filter>" + Environment.NewLine +
                "    </link-entity>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";
            Target.Id = service.Create(Target);
        } 
    }
}