using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EdgeNetworkInfrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
            try
            {

            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyConflictException("The record was modified by another process. Please try again.");
            }
        }
    }
}
