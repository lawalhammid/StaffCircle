using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.UOF
{
    /// <summary>
    /// for unit of Work
    /// </summary>
    public interface IUnitOfWork
    {
        Task<int> Save();
        Task<int> SaveAuditTrail(string UserIdOrEmail);
    }
}
