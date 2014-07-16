using System;
using System.Net;
using System.IO;
using System.Web;
using ServiceStack.Text;
using Terradue.Portal;

namespace Terradue.Github {

    [EntityTable("usr_github", EntityTableConfiguration.Custom)]
    public class GithubProfile : Entity {

        public new int Id { 
            get { 
                return base.Id;
            } 
            set { 
                base.Id = value;
            } 
        }

        private GithubClient client { get; set; }
        public GithubClient Client { 
            get{ 
                if (client == null) {
                    client = new GithubClient(context.BaseUrl, 
                                              context.GetConfigValue("Github-client-name"), 
                                              context.GetConfigValue("Github-client-id"), 
                                              context.GetConfigValue("Github-client-secret"));
                }
                return client;
            }
            set{ 
                client = value;
            }
        } 

        [EntityDataField("username")]
        public string GithubName { get; set; }

        [EntityDataField("token")]
        public string Token { get; set; }

        public bool HasSSHKey { get; set; }

        public GithubProfile(IfyContext context, int usrid) : base(context) {
            this.Id = usrid;
        }

        public static GithubProfile FromId(IfyContext context, int userId){
            GithubProfile result = new GithubProfile(context, userId);
            result.Load();
            return result;
        }

        public bool IsAuthorizationTokenValid(){
            return this.Client.ValidateAuthorizationToken(this.Token);
        }

        public void GetNewAuthorizationToken(string password, string scopes, string note){
            this.Token = this.Client.GetAuthorizationToken(this.GithubName, password, scopes, note);
            this.Store();
        }

        public void AddSSHKey(string title, string key){
            this.Client.AddSshKey(title, key);
        }

    }
}

