using System;
using System.Net;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;

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
            HttpWebRequest request = CreateWebRequest ("https://github.com/login/oauth/access_token", "POST");
            request.Accept = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.ClientId + ":" + this.ClientSecret)));

            GithubTokenRequest gRequest = new GithubTokenRequest();
            gRequest.client_id = ClientId;
            gRequest.client_secret = ClientSecret;
            gRequest.code = code;

            string json = JsonSerializer.SerializeToString<GithubTokenRequest>(gRequest);

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        string result = streamReader.ReadToEnd ();
                        try {
                            GithubAccessTokenResponse response = JsonSerializer.DeserializeFromString<GithubAccessTokenResponse> (result);
                            token = response.access_token;
                        } catch (Exception e) {
                            throw new Exception (result);
                        }
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
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/applications/" + ClientId + "/tokens/" + token, "GET");
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(this.ClientId + ":" + this.ClientSecret)));

            try{
                using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        string result = streamReader.ReadToEnd ();
                        GithubTokenResponse response = JsonSerializer.DeserializeFromString<GithubTokenResponse> (result);
                        isValid = (token.Equals (response.token));
                    }
                }
            }catch(Exception){
                return false;
            }
            return isValid;
        }
         
        /// <summary>
        /// Gets the SSH keys private.
        /// </summary>
        /// <returns>The SSH keys private.</returns>
        /// <param name="token">Token.</param>
        public List<GithubKeyResponse> GetSSHKeysPrivate(string token){
            List<GithubKeyResponse> result = new List<GithubKeyResponse>();
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/user/keys?access_token=" + token, "GET");

            using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                    string json = streamReader.ReadToEnd ();
                    result = JsonSerializer.DeserializeFromString<List<GithubKeyResponse>> (json);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the SSH keys public.
        /// </summary>
        /// <returns>The SSH keys public.</returns>
        /// <param name="username">Username.</param>
        public List<GithubKeyResponse> GetSSHKeysPublic(string username){
            List<GithubKeyResponse> result = new List<GithubKeyResponse>();
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/users/" + username + "/keys" + "?client_id=" + ClientId + "&client_secret=" + ClientSecret, "GET");

            using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                    string json = streamReader.ReadToEnd ();
                    result = JsonSerializer.DeserializeFromString<List<GithubKeyResponse>> (json);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether this user has the ssh key registered on its github profile
        /// </summary>
        /// <returns><c>true</c> if this instance has key the specified key token username; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        /// <param name="token">Token.</param>
        /// <param name="username">Username.</param>
        public bool HasKey(string key, string token, string username){
            List<GithubKeyResponse> result;
            result = GetSSHKeysPublic(username);
            foreach (GithubKeyResponse rkey in result)
                if (rkey.key.Equals(key)) return true;
            return false;
        }

        /// <summary>
        /// Adds the ssh key.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="key">Key.</param>
        /// <param name="token">Token.</param>
        public void AddSshKey(string title, string key, string token){
            HttpWebRequest request = CreateWebRequest (this.ApiBaseUrl + "/user/keys?access_token=" + token, "POST");

            GithubKeyRequest gRequest = new GithubKeyRequest();
            gRequest.title = title;
            gRequest.key = key;

            string json = JsonSerializer.SerializeToString<GithubKeyRequest>(gRequest);

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        string result = streamReader.ReadToEnd ();
                    }
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
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/users/" + githubName + "?client_id=" + ClientId + "&client_secret=" + ClientSecret, "GET");

            using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                    string json = streamReader.ReadToEnd ();
                    result = JsonSerializer.DeserializeFromString<GithubUserResponse> (json);
                }
            }
            return result;
        }
    }
}

