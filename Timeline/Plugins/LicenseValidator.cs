﻿using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Timeline.Plugins
{
    public class LicenseValidator
    {
        private readonly IOrganizationService service;
        private const Int32 Valid = 922680000;
        private const Int32 NotValid = 922680001;
        private const Int32 Expired = 922680002;

        public LicenseValidator(IOrganizationService service)
        {
            this.service = service;
        }

        public bool ValidateLicense()
        {
            // Retrieving existing TimelineConfiguration entity record values
            var query = new QueryExpression("xrmc_timelineconfiguration");
            var cols = new ColumnSet { AllColumns = true };

            query.ColumnSet = cols;
            query.PageInfo.ReturnTotalRecordCount = true;
            EntityCollection qresult = service.RetrieveMultiple(query);

            if (qresult.TotalRecordCount != 1)
            {
                return false;
            }
            var entity = qresult[0];

            var licenseManager = new LicenseManager("", LicenseDetails.PublicKey.DecodeFrom64(), LicenseDetails.DeveloperKey.DecodeFrom64());

            try
            {
                if (entity.Attributes.Contains("xrmc_licensekey"))
                {
                    var key = entity.Attributes["xrmc_licensekey"].ToString();
                    LicenseDetails licenseDetails = licenseManager.ValidateLicense(key);

                    if (entity.LogicalName == "xrmc_timelineconfiguration")
                    {
                        // Licensekey Validation
                        var keystatus = Convert.ToInt32(licenseDetails.Status);
                        if (keystatus == NotValid)
                        {
                            throw new InvalidPluginExecutionException("The license key for the Timeline solution is not valid, please contact support@xrmconsultancy.com");
                        }
                        if (keystatus == Expired)
                        {
                            throw new InvalidPluginExecutionException("The license key for the Timeline solution has expired, please contact support@xrmconsultancy.com");
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Timeline configurarion entity is not found");
                    }
                }
                else
                {
                    return false;
                }

            } //try
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred validating the license key.", ex);
            }
            return true;
        }

    }
}
