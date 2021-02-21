using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchCount.Service
{
    public abstract class SearchCountService : ISearchCountService
    {
        public async Task<List<int>> CountSearch(string searchText, string url)
        {
            var htmlContent = await GetSearchResults(searchText);
            var results = GetCount(htmlContent, url);
            return results;
        }

        public abstract Task<string> GetSearchResults(string searchText);
        

        public abstract List<int> GetCount(string htmlContent, string url);


        

    }
}
