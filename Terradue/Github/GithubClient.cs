using System;
using System.Net;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;

namespace Terradue.Github {

    public class GithubClient {

        public string BaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        private string ClientName { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        public string AccessToken { get; set; }

        public GithubClient(string baseurl){
            this.BaseUrl = baseurl;
            this.ApiBaseUrl = "https://api.github.com";
        }

        public GithubClient(string baseurl, string name, string clientid, string clientsecret) : this(baseurl){
            ClientName = name;
            ClientId = clientid;           
            ClientSecret = clientsecret;
        }

        /// <summary>
        /// Gets the authorization token.
        /// </summary>
        /// <returns>The authorization token.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="scopes">Scopes.</param>
        /// <param name="note">Note.</param>
        public string GetAuthorizationToken(string username, string password, string scopes, string note){

            string token = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/authorizations/clients/" + ClientId);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password)));

            string json = "{" +
                "\"client_secret\":\"" + ClientSecret+"\"," +
                "\"scopes\": [\"" + scopes + "\"]," +
                "\"note\":\"" + note + "\"" +
                "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    GithubTokenResponse response = JsonSerializer.DeserializeFromString<GithubTokenResponse>(result);
                    token = response.token;
                }
            }
            return token;
        }

        /// <summary>
        /// Validates the authorization token.
        /// </summary>
        /// <returns><c>true</c>, if authorization token was validated, <c>false</c> otherwise.</returns>
        /// <param name="token">Token.</param>
        public bool ValidateAuthorizationToken(string token){
            bool isValid = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/applications/" + ClientId + "/tokens/" + token);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.ClientId + ":" + this.ClientSecret)));

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string result = streamReader.ReadToEnd();
                GithubTokenResponse response = JsonSerializer.DeserializeFromString<GithubTokenResponse>(result);
                isValid = (token.Equals(response.token));
            }
            return isValid;
        }
            
        public List<GithubKeyResponse> GetSSHKeys(){
            List<GithubKeyResponse> result = new List<GithubKeyResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/user/keys?access_token=" + this.AccessToken);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string json = streamReader.ReadToEnd();
                result = JsonSerializer.DeserializeFromString<List<GithubKeyResponse>>(json);
            }
            return result;
        }

        public bool HasKey(string key){
            List<GithubKeyResponse> result = GetSSHKeys();
            foreach (GithubKeyResponse rkey in result)
                if (rkey.key.Equals(key)) return true;
            return false;
        }

        public void AddSshKey(string title, string key){
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.ApiBaseUrl + "/user/key?access_token=" + this.AccessToken);
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
                }
            }
        }

    }
}

