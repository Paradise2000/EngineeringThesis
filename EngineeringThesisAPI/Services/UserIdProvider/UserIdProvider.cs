using System.Security.Claims;

namespace EngineeringThesisAPI.Services.UserIdProvider
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserIdProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if(userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Unauthorized access");
            }

            return int.Parse(userIdClaim.Value);
        }

        public bool IsUserLogged()
        {
            if(_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
