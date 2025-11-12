using QMS.Core.Models;
using QMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.InternalTypeTestRepo
{
   public interface IInternalTypeTestRepository
    {
        Task<OperationResult> InsertInternalTypeTestAsync(InternalTypeTestViewModel model);
    }
}
