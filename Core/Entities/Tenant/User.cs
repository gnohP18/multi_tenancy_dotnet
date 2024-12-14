using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Core.Entities.Tenant
{
    [Table("users")]
    public class User : BaseModel
    {
        public string Name { get; set; } = String.Empty;
    }
}