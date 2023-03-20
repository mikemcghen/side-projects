using RestSharp;
using System.Net.Http;
using System.Web;
using TEbucksServer.Utility;

namespace TEbucksServer.Services
{
    public class ApiService
    {
        protected static RestClient client = null;


        public ApiService(string apiUrl)
        {
            if(client == null)
            {
                client = new RestClient(apiUrl);

            }
        }
    }
}
