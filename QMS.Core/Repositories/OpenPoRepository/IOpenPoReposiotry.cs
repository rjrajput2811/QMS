using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Core.Repositories.OpenPoRepository
{
    public interface IOpenPoReposiotry
    {
        Task<List<Open_PoViewModel>> GetListAsync();
        Task<BulkCreateLogResult> BulkCreateAsync(List<Open_PoViewModel> listOfData, string fileName, string uploadedBy);
    }
}
