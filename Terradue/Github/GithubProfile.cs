using System;
using System.Net;
using System.IO;
using System.Web;
using ServiceStack.Text;
using Terradue.Portal;

namespace Terradue.Github {


    [EntityTable("usr_github", EntityTableConfiguration.Custom, HasOwnerReference=true)]
    public class GithubProfile : Entity {

        public GithubClient Client { get; set; } 

        [EntityDataField("username")]
        public string GithubName { get; set; }

        [EntityDataField("token")]
        public string Token { get; set; }

        public bool HasSSHKey { get; set; }

        public GithubProfile(IfyContext context) : base(context) {
            this.Client = new GithubClient(context.BaseUrl, context.GetConfigValue("Github-client-name"), context.GetConfigValue("Github-client-id"), context.GetConfigValue("Github-client-secret"));
        }

        public GithubProfile(IfyContext context, GithubClient Application) : base(context) {
            this.Client = Application;
        }

        public static GithubProfile FromUserId(IfyContext context, int userId){
            GithubProfile result = new GithubProfile(context);
            result.UserId = userId;
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



    }
}

