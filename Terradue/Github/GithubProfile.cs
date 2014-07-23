using System;
using System.Net;
using System.IO;
using System.Web;
using ServiceStack.Text;
using Terradue.Portal;
using Terradue.Security.Certification;

namespace Terradue.Github {

    [EntityTable("usr_github", EntityTableConfiguration.Custom, HasAutomaticIds=false)]
    public class GithubProfile : Entity {

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
        public new string Name { get; set; }

        [EntityDataField("token")]
        public string Token { get; set; }

        public string Avatar { get; set; }
        public string PublicSSHKey { get; set; }
        public bool HasSSHKey { 
            get{ 
                if(this.PublicSSHKey == null) return false;
                return this.Client.HasKey(this.PublicSSHKey, this.Token, this.Name);
            } 
        }

        public GithubProfile(IfyContext context, int usrid) : base(context) {
            this.Id = usrid;
        }

        public static GithubProfile FromId(IfyContext context, int userId){
            GithubProfile result = new GithubProfile(context, userId);
            result.Load();
            return result;
        }

        public void Store(int id){
            this.Id = id;
            base.Store();
        }

        public bool IsAuthorizationTokenValid(){
            return this.Client.ValidateAuthorizationToken(this.Token);
        }

        public void GetNewAuthorizationToken(string password, string scopes, string note){
            this.Token = this.Client.GetAuthorizationToken(this.Name, password, scopes, note);
            this.Store();
        }

        public void AddSSHKey(){
            this.Client.AddSshKey("Terradue Certificate", this.PublicSSHKey, this.Token);
        }

        public void AddSSHKey(string title, string key){
            this.Client.AddSshKey(title, key, this.Token);
        }

        public override void Load(){
            base.Load();

            //Public ssh key
            CertificateUser cert = CertificateUser.FromId(context, this.Id);
            this.PublicSSHKey = cert.PubCertificateContent;

            //Github information
            if (this.Name != null) {
                try{
                    GithubUserResponse usr = this.Client.GetUser(this.Name);
                    this.Identifier = usr.id.ToString();
                    this.Avatar = usr.avatar_url;
                    // ...
                    // we can get more if we want to

                }catch(Exception e){
                }
            }
        }

    }
}

