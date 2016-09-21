using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Terradue.Github {
    public partial class GithubClient {

        public List<GithubRepositoryResponse> GetRepos(string org, string token){
            List<GithubRepositoryResponse> repos = new List<GithubRepositoryResponse>();
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/orgs/" + org + "/repos?access_token=" + token, "GET");

            try{
                using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        string result = streamReader.ReadToEnd ();
                        repos = JsonSerializer.DeserializeFromString<List<GithubRepositoryResponse>> (result);
                    }
                }
            }catch(Exception e){
                throw e;
            }
            return repos;
        }

        public GithubRepositoryResponse CreateRepo(string org, GithubRepositoryResponse repo, string token){
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/orgs/" + org + "/repos?access_token=" + token, "POST");

            GithubRepositoryResponse gResponse = null;
            GithubRepositoryRequest gRequest = new GithubRepositoryRequest();
            gRequest.name = repo.name;
            gRequest.description = repo.description;
            gRequest.Private = repo.Private;

            string json = JsonSerializer.SerializeToString<GithubRepositoryRequest>(gRequest);

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                using (var httpResponse = (HttpWebResponse)request.GetResponse ()) {
                    using (var streamReader = new StreamReader (httpResponse.GetResponseStream ())) {
                        string result = streamReader.ReadToEnd ();
                        gResponse = JsonSerializer.DeserializeFromString<GithubRepositoryResponse> (result);
                    }
                }
            }
            return gResponse;
        }

    }


}

