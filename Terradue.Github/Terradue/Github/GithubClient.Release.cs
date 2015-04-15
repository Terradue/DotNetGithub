using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Terradue.Github {
    public partial class GithubClient {

        public List<GithubReleaseResponse> GetReleases(string org, string repo, string token){
            List<GithubReleaseResponse> repos = new List<GithubReleaseResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/repos/" + org + "/" + repo + "/releases?access_token=" + token);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;

            try{
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    repos = JsonSerializer.DeserializeFromString<List<GithubReleaseResponse>>(result);
                }
            }catch(Exception e){
                throw e;
            }
            return repos;
        }

    }
}

