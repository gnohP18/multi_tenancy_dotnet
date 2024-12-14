using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tenant_service.Core.Entities.Landlord
{
    [Table("tenants")]
    public class Tenant : BaseModel
    {
        public string TenantId { get; set; } = null!;
        public string ApiTenantId { get; set; } = null!;
        public string Domain { get; set; } = null!;

        [DataType(DataType.Text)]
        public string? Note { get; set; }
    }
}