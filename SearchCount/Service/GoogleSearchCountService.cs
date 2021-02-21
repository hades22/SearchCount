using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;

namespace SearchCount.Service
{
    public class GoogleSearchCountService: SearchCountService
    {
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _clientFactory;

        public GoogleSearchCountService(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;
            _clientFactory = clientFactory;
        }

        public override async Task<string> GetSearchResults(string searchText)
        {
            ObjectCache cache = MemoryCache.Default;
            string content = cache["google-search-" + searchText] as string;
            if (content != null)
                return content;

            var decodedText = HttpUtility.UrlDecode(searchText);
            var encodedText = HttpUtility.UrlEncode(decodedText);
            var numberOfRows = Configuration["NumberOfRows"];
            var url = string.Format(Configuration["GoogleSearchUrl"], encodedText, numberOfRows);
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
                cache.Set("google-search-" + searchText, responseString, policy);
                return responseString;
            }
            return "";
        }

        public override List<int> GetCount(string htmlContent, string url)
        {
            var decodedUrl = HttpUtility.UrlDecode(url);
            if (string.IsNullOrEmpty(htmlContent))
                return new List<int>() { 0 };

            //var source = WebUtility.HtmlDecode(htmlContent);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);
            var div = htmlDoc.DocumentNode.DescendantNodes().Where(node => node.Name == "div" && node.Attributes["class"] != null).ToList().ToLookup(node => node.Attributes["class"].Value);
            var h3 = htmlDoc.DocumentNode.DescendantNodes().Where(node => node.Name == "h3").ToList();
            var result = new List<int>();
            int line = 0;
            //var results = htmlDoc.DocumentNode.DescendantNodes().Where(node => node.Name == "div" && node.Attributes["class"] != null && node.Attributes["class"].Value == id).ToList();
            foreach(var node in h3)
            {
                line++;
                var a = node.ParentNode;
                if (a.Attributes["href"] != null)
                {
                    var link = a.Attributes["href"].Value;
                    if (link.Contains(decodedUrl))
                        result.Add(line);
                }
            }

            return result.Any() ? result : new List<int>() { 0 };
        }
    }
}
