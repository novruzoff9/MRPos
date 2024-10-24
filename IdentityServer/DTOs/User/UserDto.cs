using System.Collections.Generic;

namespace IdentityServer.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public List<string> Roles { get; set; }
        
    }
}
