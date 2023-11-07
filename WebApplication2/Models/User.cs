namespace WebApplication2.Models
{
    public class User
    {
        public string Company { get; set; }
        public int CompanyID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int ID { get; set; }
        public DateTime LastAuthorize { get; set; }
        public string LastAuthorizeIP { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Picture { get; set; }
        public int Status { get; set; } /////
        public string UiLanguage { get; set; }  /////
    }
}
