using System;
using System.Net;
using System.IO;
using System.Web;
using ServiceStack.Text;
using Terradue.Portal;
using Terradue.Security.Certification;
using System.Collections.Generic;
using Terradue.Github.Reponse;
using System.Data;

namespace Terradue.Github {

    [EntityTable("usr_github", EntityTableConfiguration.Custom, HasAutomaticIds=false, NameField="username")]
    public class GithubProfile : Entity {
    
        private GithubClient Client { get; set; } 

        [EntityDataField("token")]
        public string Token { get; set; }

        public string Avatar { get; set; }
        public string PublicSSHKey { get; set; }
        public bool HasSSHKey { 
            get{ 
                try{
                    if(this.PublicSSHKey == null) return false;
                    return this.Client.HasKey(this.PublicSSHKey, this.Token, this.Name);
                }catch(Exception){
                    return false;
                }
            } 
        }

        public GithubProfile(IfyContext context) : base(context) {
            this.Client = new GithubClient(context.BaseUrl, 
                                      context.GetConfigValue("Github-client-name"), 
                                      context.GetConfigValue("Github-client-id"), 
                                      context.GetConfigValue("Github-client-secret"));
        }

        public GithubProfile(IfyContext context, int usrid) : this(context) {
            this.Id = usrid;
        }

        public GithubProfile(IfyContext context, string name) : this(context) {
            this.Name = name;
        }

        public override string AlternativeIdentifyingCondition{
            get { 
                if (Name != null) return String.Format("t.username='{0}'",Name); 
                return null;
            }
        }

        public static GithubProfile FromId(IfyContext context, int userId){
            GithubProfile result = new GithubProfile(context, userId);
            result.Load();
            return result;
        }

        public static GithubProfile FromUsername(IfyContext context, string name) {
            GithubProfile result = new GithubProfile(context, name);
            //result.Load();
            string sql = String.Format("SELECT id, token FROM usr_github WHERE username='{0}';", name);
            IDbConnection dbConnection = context.GetDbConnection();
            IDataReader reader = context.GetQueryResult(sql, dbConnection);

            if (reader.Read ()) {
                result.Id = reader.GetInt32(0);
                result.Token = reader.GetString(1);
            }
            context.CloseQueryResult(reader, dbConnection);
            return result;
        }

        public void Store(int id){
            this.Id = id;
            base.Store();
        }

        public bool IsAuthorizationTokenValid(){
            if (this.Token == null) return false;
            bool isValid = this.Client.ValidateAuthorizationToken(this.Token);
            if (!isValid) {
                this.Token = null;
                this.Store();
            }
            return isValid;
        }

        public void GetNewAuthorizationToken(string code){
            this.Token = this.Client.GetAuthorizationToken(this.Name, code);
            this.Store();
        }

        public override void Load(){
            base.Load();

            //Public ssh key
            CertificateUser cert = CertificateUser.FromId(context, this.Id);
            try{
                this.PublicSSHKey = cert.PubCertificateContent;
            }catch(Exception e){
            }

            //Github information
            if (this.Name != null) {
                try{
                    GithubUserResponse usr = this.Client.GetUser(this.Name);
                    this.Identifier = usr.id.ToString();
                    this.Avatar = usr.avatar_url;
                    // ...
                    // we can get more if we want to

                }catch(Exception e){
                    this.Avatar = null;
                    this.Identifier = null;
                }
            }
        }
    }
}

