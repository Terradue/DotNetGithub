//
//  GithubClient.cs
//
//  Author:
//       Enguerran Boissier <enguerran.boissier@terradue.com>
//
//  Copyright (c) 2014 Terradue

using System;
using System.Net;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
using Terradue.Portal;
using System.Text;

/*!

\defgroup GithubClient GithubClient
@{

\ingroup "Community"

Github Client makes requests to the interface exposed by Github platform.
It performs all requests related to Users, Authentication, Repositories, Releases.

\xrefitem api "API" "API" [GitHub API v3](https://developer.github.com/v3/)

@}
*/


namespace Terradue.Github {

    public partial class GithubClient {

        public string BaseUrl { get; set; }
        public string ApiBaseUrl { get; set; }
        private string ClientName { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }

        public GithubClient(IfyContext context) : this(context.BaseUrl, 
                                                       context.GetConfigValue("Github-client-name"), 
                                                       context.GetConfigValue("Github-client-id"), 
                                                       context.GetConfigValue("Github-client-secret")){}

        public GithubClient(string baseurl){
            this.BaseUrl = baseurl;
            this.ApiBaseUrl = "https://api.github.com";
        }

        public GithubClient(string baseurl, string name, string clientid, string clientsecret) : this(baseurl){
            ClientName = name;
            ClientId = clientid;           
            ClientSecret = clientsecret;
        }

        protected string GetBasicAuthenticationSecret(string username, string password) {
            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));
        }

        public HttpWebRequest CreateWebRequest(string url, string method) {
            return CreateWebRequest(url, method, this.ClientId, this.ClientSecret);
        }

        public HttpWebRequest CreateWebRequest (string url, string method, string username, string password) { 
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
            request.Method = method;
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;
            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)) request.Headers.Add(HttpRequestHeader.Authorization, GetBasicAuthenticationSecret(username,password));

            return request;
        }
    }
}

