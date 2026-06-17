using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkApplication.Dtos.Bills;
using EdgeNetworkDomain.Common;

namespace EdgeNetworkApplication.Interface
{
    public interface IBillsServices
    {
        Task PurchaseAirtimeAsync(PurchaseAirtimeDto dto, Guid requestingUserId);
        Task PurchaseDataAsync(PurchaseDataDto dto, Guid requestingUserId);
        IEnumerable<DataPlan> GetDataPlans();

    }
}
