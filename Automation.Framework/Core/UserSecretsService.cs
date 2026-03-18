using Automation.Framework.Models;
using Microsoft.Extensions.Configuration;

namespace Automation.Framework.Core
{
    public class UserSecretsService
    {
        /// <summary>
        /// Provides access to user credentials stored in User Secrets.
        /// 
        /// This service allows tests to retrieve users by role (e.g. "Admin", "StandardUser")
        /// without hardcoding sensitive information in code or configuration files
        ///</summary>
        ///

        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, TestUser> _cache = new();

        public UserSecretsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TestUser GetUser(string role)
        {
            /// <summary>
            /// Retrieves a TestUser object for the specified role.
            /// Caches results to avoid repeated configuration lookups.
            /// </summary>
            /// 

            if (_cache.ContainsKey(role))
                return _cache[role];

            var userSection = _configuration.GetSection($"TestUsers:{role}");
            var user = userSection.Get<TestUser>();

            if (user == null || string.IsNullOrEmpty(user.Username))
                throw new Exception($"No user secrets found for role: {role}");

            _cache[role] = user;
            return user;
        }
    }
}
