using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;

namespace EdgeNetworkInfrastructure.Repositories
{
    public class BillsRepository : IBillsRepository
    {
        private readonly AppDBContext _context;

        public BillsRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BillPayment billPayment, CancellationToken ct = default)
        {
            await _context.BillPayments.AddAsync(billPayment, ct);
        }

        public void Update(BillPayment billPayment)
        {
            _context.BillPayments.Update(billPayment);
        }
    }
}
