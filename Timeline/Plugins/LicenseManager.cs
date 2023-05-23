using System;
using Microsoft.Xrm.Sdk;
using SeriousBit.Licensing;

namespace Timeline.Plugins
{
    public enum LicenseStatus
    {
        /// <remarks/>
        Valid = 922680000,

        /// <remarks/>
        NotValid = 922680001,

        /// <remarks/>
        Expired = 922680002,

    }

    public class LicenseManager
    {
        readonly string privatekey;
        readonly string publickey;
        readonly string developerkey;
        private readonly ITracingService tracingService;
        private const Int32 Valid = 922680000;
        private const Int32 NotValid = 922680001;
        private const Int32 Expired = 922680002;

        public LicenseManager(string privateKey, string publicKey, string developerKey, ITracingService tracingService = null)
        {
            privatekey = privateKey;
            publickey = publicKey;
            developerkey = developerKey;
            this.tracingService = tracingService;
        }

        public LicenseDetails ValidateLicense(string licenseKey)
        {
            var licensedetails = new LicenseDetails();
            var manager = new SerialsManager("SeriosuBit", developerkey)
                                         {
                                             PrivateKey = privatekey,
                                             PublicKey = publickey
                                         };
            Trace("Inspecting license key...");
            if (manager.IsValid(licenseKey))
            {
                Trace("License key structure is valid");
                licensedetails.Expirydate = manager.GetExpirationDate(licenseKey);
                licensedetails.Status = Valid.ToString();
                string productInfo = manager.GetInfo(licenseKey);
                if (productInfo != "")
                {
                    Trace("Parsing license key details");
                    if (productInfo.Split(':').Length > 2)
                    {
                        Trace("Valid licence key details. Extracting relevant information...");
                        licensedetails.MaxActiveUsers = Convert.ToInt32(productInfo.Split(':')[2]);
                        licensedetails.CustomerName = productInfo.Split(':')[1];
                        licensedetails.Name = productInfo.Split(':')[0];
                        licensedetails.Licensekeyid = manager.GetID(licenseKey);
                    }
                    else
                    {
                        Trace("Invalid licence key details");
                        licensedetails.Status = NotValid.ToString();
                    }
                }
                if (licensedetails.Expirydate < DateTime.Now)
                    licensedetails.Status = Expired.ToString();

            }
            else
            {
                Trace("Invalid licence key structure!");
                licensedetails.Status = NotValid.ToString();
            }
            return licensedetails;
        }

        private void Trace(string format, params object[] args)
        {
            if (tracingService != null)
            {
                tracingService.Trace(format, args);
            }
        }
    }
}
