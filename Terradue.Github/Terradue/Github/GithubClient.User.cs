using System;
using System.Net;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
using Terradue.Github.Response;
using Terradue.Github.Reponse;

namespace Terradue.Github {

    public partial class GithubClient {

        /// <summary>
        /// Gets the authorization token.
        /// </summary>
        /// <returns>The authorization token.</returns>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="scopes">Scopes.</param>
        /// <param name="note">Note.</param>
        public string GetAuthorizationToken(string username, string code){
            if (username == null)
                throw new Exception("User github name not set");
            string token = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://github.com/login/oauth/access_token");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.UserAgent = this.ClientName;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.ClientId + ":" + this.ClientSecret)));

            string json = "{" +
                "\"client_id\":\"" + ClientId+"\"," +
                "\"client_secret\":\"" + ClientSecret+"\"," +
                "\"code\": \"" + code + "\"" +
                "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    try{
                        GithubAccessTokenResponse response = JsonSerializer.DeserializeFromString<GithubAccessTokenResponse>(result);
                        token = response.access_token;
                    }catch(Exception e){
                        throw new Exception(result);
                    }
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
            if (token == null) return false;

            bool isValid = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/applications/" + ClientId + "/tokens/" + token);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.ClientId + ":" + this.ClientSecret)));

            try{
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    GithubTokenResponse response = JsonSerializer.DeserializeFromString<GithubTokenResponse>(result);
                    isValid = (token.Equals(response.token));
                }
            }catch(Exception){
                return false;
            }
            return isValid;
        }
            
        public List<GithubKeyResponse> GetSSHKeysPrivate(string token){
            List<GithubKeyResponse> result = new List<GithubKeyResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/user/keys?access_token=" + token);
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

        public List<GithubKeyResponse> GetSSHKeysPublic(string username){
            List<GithubKeyResponse> result = new List<GithubKeyResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/users/" + username + "/keys" + "?client_id=" + ClientId + "&client_secret=" + ClientSecret);
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

        public bool HasKey(string key, string token, string username){
            List<GithubKeyResponse> result;
            result = GetSSHKeysPublic(username);
            foreach (GithubKeyResponse rkey in result)
                if (rkey.key.Equals(key)) return true;
            return false;
        }

        public void AddSshKey(string title, string key, string token){
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.ApiBaseUrl + "/user/keys?access_token=" + token);
            request.Method = "POST";
            request.ContentType = "application/json";
//            request.Headers.Add("X-OAuth-Scopes", "write:public_key,admin:org,repo");
//            request.Headers.Add("X-Accepted-OAuth-Scopes", "write:public_key,admin:org,repo");
            request.UserAgent = this.ClientName;

            string json = "{\"title\":\""+title+"\",\"key\":\""+key+"\"}";

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

        /// <summary>
        /// Gets the user github profile
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="githubName">Github name.</param>
        public GithubUserResponse GetUser(string githubName) {
            GithubUserResponse result = new GithubUserResponse();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/users/" + githubName + "?client_id=" + ClientId + "&client_secret=" + ClientSecret);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string json = streamReader.ReadToEnd();
                result = JsonSerializer.DeserializeFromString<GithubUserResponse>(json);
            }
            return result;
        }
    }
}

