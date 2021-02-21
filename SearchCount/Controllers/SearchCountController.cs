using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchCount.Service;

namespace SearchCount.Controllers
{
    [Route("api/count")]
    [ApiController]
    public class SearchCountController : ControllerBase
    {
        private readonly ISearchCountService _searchCountService;
        public SearchCountController(ISearchCountService searchCountService):base()
        {
            _searchCountService = searchCountService;
        }


        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<int>>> CountSearch(string searchText, string url)
        {
            var result = await _searchCountService.CountSearch(searchText, url);
            return Ok(result);
        }
    }
}