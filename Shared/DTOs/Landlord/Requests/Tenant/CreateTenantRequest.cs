using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Shared.DTOs.Landlord.Requests.Tenant
{
    public class CreateTenantRequest
    {
        [Required]
        public string Domain { get; set; } = null!;
        public string Note { get; set; } = null!;
    }
}