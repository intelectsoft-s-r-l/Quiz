using System.ComponentModel;

namespace WebApplication2.Models.API.License
{
    public class GetLicense : BaseErrors
    {
        public License license { get; set; }
    }
}
