using System;

namespace Timeline.Plugins
{
    public class LicenseDetails
    {
        private static string publicKey = "V1UyVERFVlg5OTg0UVFLTDhVODY0SFZNQ1VWQkpOU0ZMSktXWEFXVjZHTThHNFE=";
        private static string developerKey = "UVRaNTJHLU5HSDRMTC05WkZQOUItNVVBVEpDLTFXRUdR";

        public static string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }

        public static string DeveloperKey
        {
            get { return developerKey; }
            set { developerKey = value; }
        }

        public string Licensekey { get; set; }

        public string Name { get; set; }

        public string CustomerName { get; set; }

        public long Licensekeyid { get; set; }

        public string Status { get; set; }

        public DateTime Expirydate { get; set; }

        public int MaxActiveUsers { get; set; }

        public DateTime LastResetDate { get; set; }

    }
}
