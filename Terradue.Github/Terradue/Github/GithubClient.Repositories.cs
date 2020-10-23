using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Terradue.Github {
    public partial class GithubClient {

        public List<GithubRepositoryResponse> GetRepos(string org){
            List<GithubRepositoryResponse> repos = new List<GithubRepositoryResponse>();
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/orgs/" + org + "/repos", "GET");

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

        public GithubRepositoryResponse CreateRepo(string org, GithubRepositoryResponse repo){
            HttpWebRequest request = CreateWebRequest (ApiBaseUrl + "/orgs/" + org + "/repos", "POST");

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

        public List<GithubUserResponse> GetRepoCollaborators(string org, string repo) {
            List<GithubUserResponse> repos = new List<GithubUserResponse>();
            HttpWebRequest request = CreateWebRequest(ApiBaseUrl + "/repos/" + org + "/" + repo + "/collaborators", "GET");

            try {
                using (var httpResponse = (HttpWebResponse)request.GetResponse()) {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                        string result = streamReader.ReadToEnd();
                        repos = JsonSerializer.DeserializeFromString<List<GithubUserResponse>>(result);
                    }
                }
            } catch (Exception e) {
                throw e;
            }
            return repos;
        }

        public bool IsUserRepoCollaborators(string org, string repo, string username) {
            bool result = false;
            HttpWebRequest request = CreateWebRequest(ApiBaseUrl + "/repos/" + org + "/" + repo + "/collaborators/" + username, "GET");

            try {
                using (var httpResponse = (HttpWebResponse)request.GetResponse()) {
                    if (httpResponse.StatusCode == HttpStatusCode.NoContent) result = true;
                }
            } catch (Exception e) {
                return false;
            }
            return result;
        }

        public void AddCollaboratorToRepo(string org, string repo, string username, string permission) {
            var result = new GithubUserResponse();
            HttpWebRequest request = CreateWebRequest(ApiBaseUrl + "/repos/" + org + "/" + repo + "/collaborators/" + username, "PUT");

            string jsonbody = "{\"permission\":\""+permission+"\"}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                streamWriter.Write(jsonbody);
                streamWriter.Flush();
                streamWriter.Close();

                using (var httpResponse = (HttpWebResponse)request.GetResponse()) {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                        string json = streamReader.ReadToEnd();
                        result = JsonSerializer.DeserializeFromString<GithubUserResponse>(json);
                    }
                }
            }
        }

        public void RemoveCollaboratorFromRepo(string org, string repo, string username) {
            var result = new GithubUserResponse();
            HttpWebRequest request = CreateWebRequest(ApiBaseUrl + "/repos/" + org + "/" + repo + "/collaborators/" + username, "DELETE");

            using (var httpResponse = (HttpWebResponse)request.GetResponse()) {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    string json = streamReader.ReadToEnd();
                }
            }
        }

    }

    public class GithubRepositoryPermission {
        public static string Push = "push"; // can pull and push, but not administer this repository.
        public static string Pull = "pull"; // can pull, but not push to or administer this repository 
        public static string Admin = "admin"; // can pull, push and administer this repository.
        public static string Maintain = "maintain"; // Recommended for project managers who need to manage the repository without access to sensitive or destructive actions.
        public static string Triage = "triage"; // Recommended for contributors who need to proactively manage issues and pull requests without write access.
    }
}

