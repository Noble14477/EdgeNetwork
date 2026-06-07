using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkApplication.Dtos
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty; 
        public Guid UserId { get; set; }
    }
}
