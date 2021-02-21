using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchCount.Service
{
    public interface ISearchCountService
    {
        Task<List<int>> CountSearch(string searchText, string url);
    }
}
