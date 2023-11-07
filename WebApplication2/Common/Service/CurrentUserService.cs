using Newtonsoft.Json;
using System.Security.Claims;

namespace ISAdminWeb.Common.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {

            string userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isValidGuid = Int32.TryParse(userId, out int parserUserId);
            if (isValidGuid)
            {
                UserId = parserUserId;
            }
            //UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            FullName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            //FullName = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "FullName").Value
            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            Company = httpContextAccessor.HttpContext?.User?.FindFirstValue("Company");

            //Role = httpContextAccessor.HttpContext?.User?.Claims
            //    .Where(x => x.Type == ClaimTypes.Role)
            //    .Select(x => x.Value)
            //    .FirstOrDefault();
            //IsAuthenticated = isValidGuid;

        }

        public string Email { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public int UserId { get; }

    }
}
