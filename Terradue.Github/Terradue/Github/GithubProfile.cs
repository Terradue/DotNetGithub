using System;
using System.Net;
using System.IO;
using System.Web;
using ServiceStack.Text;
using Terradue.Portal;
using Terradue.Security.Certification;
using System.Collections.Generic;
using System.Data;


/*! 
\defgroup GithubProfile GithubProfile
@{

This component defines a the github profile of a user. It is used to link the user profile to his Github account, and also stores user tokens to enable authentication.

\ingroup User

\xrefitem dep "Dependencies" "Dependencies" calls \ref Persistence to store the object (for ex in a Mysql database)

\startuml
TODO: sequence diagram

footer
GeoHazards TEP User account activity diagram
(c) Terradue Srl
endfooter
\enduml

@}
*/

namespace Terradue.Github {

    [EntityTable("usr_github", EntityTableConfiguration.Custom, HasAutomaticIds=false, NameField="username")]
    public class GithubProfile : Entity {
    
        private GithubClient Client { get; set; } 

        [EntityDataField("token")]
        public string Token { get; set; }

        [EntityDataField("email")]
        public string Email { get; set; }

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
            try{
                GithubProfile result = new GithubProfile(context, userId);
                result.Load();
                return result;
            }catch(Exception e){
                context.LogError(typeof(Terradue.Github.GithubProfile), String.Format("{0} : {1}", e.Message, e.StackTrace));
                throw new Exception("Github account does not exist for the user.");
            }
        }

        public static GithubProfile FromUsername(IfyContext context, string name) {
            GithubProfile result = new GithubProfile(context, name);
            //result.Load();
            string sql = String.Format("SELECT id, token, email FROM usr_github WHERE username='{0}';", name);
            IDbConnection dbConnection = context.GetDbConnection();
            IDataReader reader = context.GetQueryResult(sql, dbConnection);

            if (reader.Read ()) {
                result.Id = reader.GetInt32(0);
                result.Token = reader.GetString(1);
                result.Email = reader.GetString(2);
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
            context.LogInfo(this, String.Format("Loading user account: Id={0}",this.Id));

            base.Load();

            //Github information
            if (this.Name != null) {
                context.LogInfo(this, String.Format("Loading user account: Name={0}",this.Name));
                try{
                    GithubUserResponse usr = this.Client.GetUser(this.Name);
                    this.Identifier = usr.id.ToString();
                    this.Avatar = usr.avatar_url;
                    this.Email = usr.email;
                    // ...
                    // we can get more if we want to

                    //we store in case of changes
                    this.Store();
                }catch(Exception e){
                    this.Avatar = null;
                    this.Identifier = null;
                    context.LogError(this, String.Format("{0} : {1}", e.Message, e.StackTrace));
                }
            }
        }

        /// <summary>
        /// Loads the public key from certificate.
        /// </summary>
        public void LoadPublicKeyFromCertificate(){
            //Public ssh key
            CertificateUser cert = CertificateUser.FromId(context, this.Id);
            try{
                this.PublicSSHKey = cert.PubCertificateContent;
            }catch(Exception e){
                context.LogError(this, String.Format("{0} : {1}", e.Message, e.StackTrace));
            }
        }

        /// <summary>
        /// Loads the public key from safe.
        /// </summary>
        public void LoadPublicKeyFromSafe(){
            Safe safe = Safe.FromUserId(context, this.UserId);
            try{
                this.PublicSSHKey = safe.GetBase64SSHPublicKey();
            }catch(Exception e){
                context.LogError(this, String.Format("{0} : {1}", e.Message, e.StackTrace));
            }
        }
    }
}

