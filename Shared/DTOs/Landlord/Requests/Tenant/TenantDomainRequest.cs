using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Shared.DTOs.Landlord.Requests.Tenant
{
    public class TenantDomainRequest
    {
        [Required(ErrorMessage = "Domain is required")]
        public string Domain { get; set; } = String.Empty;
    }
}