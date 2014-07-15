using System;
using System.Net;
using System.IO;
using System.Web;

namespace Terradue.Github {

    public class GithubApplication {

        public string BaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        private string ClientSecret { get; set; }
        private string ClientId { get; set; }
        private string token { get; set; }
        public string AccessToken { 
            get{ 
                if (token == null) {
                    token = this.GetAuthorizationToken();
                }
                return token;
            }
        }


        public GithubApplication(string baseurl){
            this.BaseUrl = baseurl;
            this.ApiBaseUrl = "https://api.github.com";
        }

        public GithubApplication(string baseurl, string client, string token) : this(baseurl){
            ClientId = client;           
            ClientSecret = token;
        }

        public string GetAuthorization(){
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://github.com/login/oauth/authorize" + 
                                                                       "?client_id=" + ClientId +
                                                                       "&scope=user,write:public_key" + 
                                                                       "&redirect_uri=" + HttpContext.Current.Request.Url.AbsoluteUri);

            request.Method = "GET";
            request.ContentType = "application/json";

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string result = streamReader.ReadToEnd();
                Console.WriteLine("GITHUB result is : "+result);
                return result;
            }
        }

        public string GetAuthorizationToken(){
            string token = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/authorizations/clients/" + ClientId);
            request.Method = "PUT";
            request.ContentType = "application/json";

            string json = "{" +
                "\"client_secret\":\"" + ClientSecret+"\"," +
                "\"scopes\": [\"write:public_key\"]," +
                "\"note\":\"Terradue ssh keys management\"" +
                "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    Console.WriteLine("GITHUB result is : "+result);
                    token = "";
                }
            }
            return token;
        }
    }

    public class GithubProfile {

        protected GithubApplication Application { get; set; } 
        public string Username { get; set; }

        public GithubProfile(GithubApplication Application) {
            this.Application = Application;
        }

        public void AddSshKey(string title, string key){

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Application.ApiBaseUrl + "/user/keys?"
                                                                       + "access_token=" + "ac0453a684c0b9c12633ac893e6568e986cab55f");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("X-OAuth-Scopes", "user,write:public_key");
            request.Headers.Add("X-Accepted-OAuth-Scopes", "user,write:public_key");

            string json = "{" +
                "\"title\":\""+title+"\"," +
                "\"key\":\""+key+"\"" +
                "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    Console.WriteLine("GITHUB result is : "+result);
                }
            }
        }

    }
}

