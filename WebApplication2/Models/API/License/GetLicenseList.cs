using System.ComponentModel;

namespace WebApplication2.Models.API.License
{
    public class GetLicenseList : BaseErrors
    {
        public List<License> licenses { get; set; }
    }
}
