using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SMSManager
{
    public class SMSService
    {
        private const string NetworkInfoUrlPath = "services/api/status/network";

        private const string BatteryInfoUrlPath = "services/api/status/battery";

        private const string MessagesUrlPath = "services/api/messaging";

        private const string MessageStatusUrlPath = "services/api/messaging/status";

        private string IP;
        private int Port;
        private string Username;
        private string Password;

        public SMSService(string IP, int Port, string Username, string Password)
        {
            this.IP = IP;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
        }

        public string ConstructBaseUri()
        {
            UriBuilder uriBuilder = new UriBuilder("http", this.IP, Port);
            return uriBuilder.ToString();
        }

        public async Task<PostMessageResponse> Send(string contact, string message)
        {
            using (var client = new HttpClient())
            {
                string url = ConstructBaseUri();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                                "Basic",
                                 Convert.ToBase64String(
                                 ASCIIEncoding.ASCII.GetBytes(
                                 string.Format("{0}:{1}", Username, Password))));
                }

                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("to", contact));
                postData.Add(new KeyValuePair<string, string>("message", message));
                HttpContent content = new FormUrlEncodedContent(postData);
                
                HttpResponseMessage response = await client.PostAsync(MessagesUrlPath, content);
                PostMessageResponse result = new PostMessageResponse();
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<PostMessageResponse>();
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Description = "HttpResponseMessage Error";
                }
                return result;
            }
        }
    }
}
