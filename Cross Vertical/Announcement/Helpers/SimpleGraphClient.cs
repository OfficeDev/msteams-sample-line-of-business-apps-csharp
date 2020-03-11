using CrossVertical.Announcement.Helper;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CrossVertical.Announcement.Helpers
{
    public class GraphHelper
    {
        private readonly string _token;

        public GraphHelper(string token)
        {
            _token = token;
        }

        public static async Task<string> GetAccessToken(string tenant, string appId, string appSecret)
        {
            string response = await POST($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token",
                              $"grant_type=client_credentials&client_id={appId}&client_secret={appSecret}"
                              + "&scope=https%3A%2F%2Fgraph.microsoft.com%2F.default");

            string accessToken = JsonConvert.DeserializeObject<TokenResponse>(response).access_token;
            return accessToken;
        }

        // Get a specified user's photo.
        public async Task<User> GetUserFromDisplayName(string userName)
        {
            var graphClient = GetAuthenticatedClient();
            // Get the user.
            try
            {
                var filteredUsers = await graphClient.Users.Request()
                .Filter($"startswith(displayName,'{userName}') or startswith(givenName,'{userName}') or startswith(surname,'{userName}') or startswith(mail,'{userName}') or startswith(userPrincipalName,'{userName}')")
                .GetAsync();
                return filteredUsers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return null;
        }

        public async Task<User> GetUser(string userId)
        {
            var graphClient = GetAuthenticatedClient();
            // Get the user.
            try
            {
                var filteredUsers = await graphClient.Users.Request()
                .Filter($"startswith(userPrincipalName,'{userId}')")
                .GetAsync();
                return filteredUsers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return null;
        }

        public async Task<string> GetUserProfilePhoto(string tenantId, string userId)
        {
            var graphClient = GetAuthenticatedClient();
            var profilePhotoUrl = string.Empty;
            try
            {
                var baseDirectory = $"/ProfilePhotos/{tenantId}/";
                var fileName = userId + ".png";
                string imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~" + baseDirectory);
                if (!System.IO.Directory.Exists(imagePath))
                    System.IO.Directory.CreateDirectory(imagePath);
                imagePath += fileName;

                if (System.IO.File.Exists(imagePath))
                    return ApplicationSettings.BaseUrl + baseDirectory + fileName;

                var photo = await graphClient.Users[userId].Photo.Content.Request().GetAsync();
                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    photo.Seek(0, SeekOrigin.Begin);
                    photo.CopyTo(fileStream);
                }
                profilePhotoUrl = ApplicationSettings.BaseUrl + baseDirectory + fileName;
            }
            catch (Exception ex)
            {
                ErrorLogService.LogError(ex);
                profilePhotoUrl = ApplicationSettings.BaseUrl + "/Resources/Person.png";
            }
            return profilePhotoUrl;
        }

        public async Task<string> GetTeamPhoto(string tenantId, string teamId)
        {
            var graphClient = GetAuthenticatedClient();
            var profilePhotoUrl = string.Empty;
            try
            {
                var baseDirectory = $"/ProfilePhotos/{tenantId}/";
                var fileName = teamId + ".png";
                string imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~" + baseDirectory);
                if (!System.IO.Directory.Exists(imagePath))
                    System.IO.Directory.CreateDirectory(imagePath);
                imagePath += fileName;

                if (System.IO.File.Exists(imagePath))
                    return ApplicationSettings.BaseUrl + baseDirectory + fileName;

                var photo = await graphClient.Groups[teamId].Photo.Content.Request().GetAsync();
                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    photo.Seek(0, SeekOrigin.Begin);
                    photo.CopyTo(fileStream);
                }
                profilePhotoUrl = ApplicationSettings.BaseUrl + baseDirectory + fileName;
            }
            catch (Exception ex)
            {
                ErrorLogService.LogError(ex);
                profilePhotoUrl = ApplicationSettings.BaseUrl + "/Resources/Team.png";
            }
            return profilePhotoUrl;
        }


        public static async Task<string> POST(string url, string body)
        {
            HttpClient httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseBody);
            return responseBody;
        }

        public async Task<User> GetMe()
        {
            var graphClient = GetAuthenticatedClient();
            var me = await graphClient.Me.Request().GetAsync();
            return me;
        }

        public async Task<System.IO.Stream> GetProfilePhoto()
        {
            var graphClient = GetAuthenticatedClient();
            var me = await graphClient.Me.Photo.Content.Request().GetAsync();
            return me;
        }

        public async Task<User> GetManager()
        {
            var graphClient = GetAuthenticatedClient();
            User manager = await graphClient.Me.Manager.Request().GetAsync() as User;
            return manager;
        }

        private GraphServiceClient GetAuthenticatedClient()
        {
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        string accessToken = _token;

                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                        // Get event times in the current time zone.
                        requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");
                    }));
            return graphClient;
        }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
    }
}