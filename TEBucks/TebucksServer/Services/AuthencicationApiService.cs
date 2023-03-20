using RestSharp.Authenticators;
using RestSharp;
using TEBucksServer.Models;

namespace TEbucksServer.Services
{
    public class AuthencicationApiService : ApiService
    {
        private static ReturnUser user = new ReturnUser();
        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }
        public AuthencicationApiService(string apiUrl) : base(apiUrl){}

        public bool Login(string submittedName, string submittedPass)
        {
            LoginUser loginUser = new LoginUser { Username = submittedName, Password = submittedPass };
            RestRequest request = new RestRequest("login");
            request.AddJsonBody(loginUser);
            IRestResponse<ReturnUser> response = client.Post<ReturnUser>(request);

            user.Token = response.Data.Token;

            // TODO: Set the token on the client
            client.Authenticator = new JwtAuthenticator(user.Token);
            return true;
        }

        public void Logout()
        {
            user = new ReturnUser();

            // TODO: Remove the token from the client
            client.Authenticator = null;
        }

    }
}
