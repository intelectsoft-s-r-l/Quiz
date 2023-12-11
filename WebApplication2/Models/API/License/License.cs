using WebApplication2.Models.Enum;

namespace WebApplication2.Models.API.License
{
    public class License
    {
        public string oid {  get; set; }  //
        public string id {  get; set; }
        public int companyOid { get; set; }
        public string activationCode { get; set; }  //d
        public string licenseActivated { get; set; } //Time
        public EnLicenseStatus licenseStatus { get; set; }  //
        public string freeRam { get; set; } //!
        public string totalRam { get; set; }    //!
        public string deviceModel { get; set; } 
        public string osVersion { get; set; }
        public int os { get; set; }
        public string deviceName { get; set; }  //
        public int battery { get; set; }    //
        public string ip { get; set; }
    }
}
