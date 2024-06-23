using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Sessions
{
    public class CustomSession : ICustomSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string UserIdKey = "UserId";
        private const string RoleIdKey = "RoleId";

        public CustomSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get => _httpContextAccessor.HttpContext.Session.GetString(UserIdKey);
            set => _httpContextAccessor.HttpContext.Session.SetString(UserIdKey, value);
        }

        public string RoleId
        {
            get => _httpContextAccessor.HttpContext.Session.GetString(RoleIdKey);
            set => _httpContextAccessor.HttpContext.Session.SetString(RoleIdKey, value);
        }
    }
}
