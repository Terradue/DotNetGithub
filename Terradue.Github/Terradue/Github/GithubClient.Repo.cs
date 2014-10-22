using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ServiceStack.Text;
using Terradue.Github.Reponse;

namespace Terradue.Github {
    public partial class GithubClient {

        public List<GithubRepositoryResponse> GetRepos(string org, string token){
            List<GithubRepositoryResponse> repos = new List<GithubRepositoryResponse>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/orgs/" + org + "/repos?access_token=" + token);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;

            try{
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    repos = JsonSerializer.DeserializeFromString<List<GithubRepositoryResponse>>(result);
                }
            }catch(Exception e){
                throw e;
            }
            return repos;
        }

        public GithubRepositoryResponse CreateRepo(string org, GithubRepositoryResponse repo, string token){
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + "/orgs/" + org + "/repos?access_token=" + token);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = this.ClientName;
            GithubRepositoryResponse response = null;
            string json = "{" +
                "\"name\":\"" + repo.name+"\"," +
                "\"description\": [" + repo.description + "]," +
                "\"private\":\"" + repo.Private + "\"" +
                "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string result = streamReader.ReadToEnd();
                    response = JsonSerializer.DeserializeFromString<GithubRepositoryResponse>(result);

                }
            }
            return response;
        }

    }
}

