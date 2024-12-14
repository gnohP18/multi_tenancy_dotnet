using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tenant_service.Shared.DTOs
{
    public class BaseRequest
    {
        [JsonIgnore]
        public int Limit { get; set; } = 15;
        [JsonIgnore]
        public int Page { get; set; } = 1;
        [JsonIgnore]
        public string SortBy { get; set; } = "Id";
        [JsonIgnore]
        public string Direction { get; set; } = "DESC";
    }
}