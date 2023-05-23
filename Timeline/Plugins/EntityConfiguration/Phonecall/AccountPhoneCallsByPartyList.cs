using System;
using Microsoft.Xrm.Sdk;

namespace Timeline.Plugins.EntityConfiguration.Phonecall
{
    class AccountPhoneCallsByPartyList : BaseLinkBuilder, IBuilder
    {
        public AccountPhoneCallsByPartyList(IOrganizationService service, IBuilder parent)
            : base(service, parent)
        {
        }

        public void Construct()
        {
            // If a config already exists simply return it rather than creating a new one
            const string linkName = "Account Phone Calls by Party List";
            Target = RetrieveExistingConfig(linkName);
            if (Target != null) return;

            Target = new Entity("xrmc_timelineentitylink");
            Target["xrmc_name"] = linkName;
            Target["xrmc_logicalentityname"] = "account";
            Target["xrmc_timelineentity"] = parent.Target.ToEntityReference();
            Target["xrmc_query"] =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" + Environment.NewLine +
                "  <entity name='activitypointer'>" + Environment.NewLine +
                "    <attribute name='activitytypecode' />" + Environment.NewLine +
                "    <attribute name='subject' />" + Environment.NewLine +
                "    <attribute name='statecode' />" + Environment.NewLine +
                "    <attribute name='statuscode' />" + Environment.NewLine +
                "    <attribute name='activityid' />" + Environment.NewLine +
                "    <attribute name='instancetypecode' />" + Environment.NewLine +
                "    <attribute name='regardingobjectid' />" + Environment.NewLine +
                "    <attribute name='scheduledstart' />" + Environment.NewLine +
                "    <attribute name='scheduledend' />" + Environment.NewLine +
                "    <attribute name='actualstart' />" + Environment.NewLine +
                "    <attribute name='actualend' />" + Environment.NewLine +
                "    <attribute name='createdon' />" + Environment.NewLine +
                "    <order attribute='actualend' descending='true' />" + Environment.NewLine +
                "    <order attribute='scheduledend' descending='true' />" + Environment.NewLine +
                "    <filter type='and'>" + Environment.NewLine +
                "        <condition attribute='activitytypecode' operator='in'>" + Environment.NewLine +
                "            <value>4210</value>" + Environment.NewLine +
                "        </condition>" + Environment.NewLine +
                "    </filter>" + Environment.NewLine +
                "    <link-entity name='activityparty' from='activityid' to='activityid' alias='ad'>" + Environment.NewLine +
                "      <filter type='and'>" + Environment.NewLine +
                "        <condition attribute='partyid' operator='eq' value='{0}' />" + Environment.NewLine +
                "      </filter>" + Environment.NewLine +
                "    </link-entity>" + Environment.NewLine +
                "  </entity>" + Environment.NewLine +
                "</fetch>";
            Target.Id = service.Create(Target);
        }
    }
}

