using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.Common
{
    public static class DataPlans
    {
        public static readonly List<DataPlan> Plans =
        [
            new("1GB-30days", "1GB - 30 Days", 300),
            new("2GB-30days", "2GB - 30 Days", 500),
            new("5GB-30days", "5GB - 30 Days", 1000),
            new("10GB-30days", "10GB - 30 Days", 2000),
            new("20GB-30days", "20GB - 30 Days", 3500),
        ];

        public static DataPlan? GetPlan(string planId) => Plans.FirstOrDefault(p => p.Id == planId);
    }

    public record DataPlan(string Id, string Description, decimal Price);
}
