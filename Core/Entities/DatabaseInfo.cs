using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Core.Entities
{
    public class DatabaseInfo
    {
        public const string Landlord = "Landlord";
        public const string Tenant = "Tenant";
        public const string MysqlInfo = "MysqlInfo";

        public string Server { get; set; } = String.Empty;
        public string Port { get; set; } = String.Empty;
        public string User { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Database { get; set; } = String.Empty;
    }
}