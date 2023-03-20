using RestSharp;
using System.Net.Http;
using TEbucksServer.Utility;

namespace TEbucksServer.Services
{
    public class LogApiService : ITxLog
    {
        protected static RestClient client = null;
        private string apiKey = "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4MDI3IiwibmFtZSI6Im1pa2UiLCJuYmYiOjE2Nzg2NDU0NDAsImV4cCI6MTY3ODczMTg0MCwiaWF0IjoxNjc4NjQ1NDQwfQ.1b3nWp-IVtvx3Llfb0bx5JZs1pKZOQRGVSoGsEdovn0";

        public LogApiService() 
        {
            if (client == null)
            {
                client = new RestClient("https://te-pgh-api.azurewebsites.net/api/TxLog");
                client.AddDefaultHeader("Authorization", apiKey);
            }
        }
        public Log AddLog(TxLog newLog)
        {
            RestRequest request = new RestRequest();
            request.AddJsonBody(newLog);
            IRestResponse<Log> response = client.Post<Log>(request);
            //CheckForError(response, $"Add Log");
            return response.Data;
            //if(response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
            //{
            //    return response.Data;
            //}
            //else
            //{
            //    return null;
            //}
        }
        private void CheckForError(IRestResponse response, string action)
        {
            string message = $"Error occured in {action}";
            bool HasError = false;
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                BasicLogger.Log($"{message} - unable to reach server: {response.ResponseStatus}");
                HasError = true;
            }
            else if (!response.IsSuccessful)
            {
                BasicLogger.Log($"{message} - http error occured: {(int)response.StatusCode} -- {response.StatusDescription}");
            }
            if (HasError)
            {
                throw new HttpRequestException($"There was an error in the call to the server");
            }
        }
    }
}
