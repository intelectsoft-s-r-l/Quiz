namespace ISAdminWeb.Common.Service
{
    public interface ICurrentUserService
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public int UserId { get; }
    }
}
