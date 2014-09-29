using System;
using System.Net;
using System.IO;
using ServiceStack.Text;
using System.Collections.Generic;
using Terradue.Portal;

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
    }
}

