using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceInterface;
using Terradue.Portal;
using Terradue.Github;

namespace Terradue.WebService.Model {

    [Route("/github/token", "PUT", Summary = "GET a new token for the user", Notes = "User is current user")]
    public class GetNewGithubToken : IReturn<bool> {
        [ApiMember(Name = "Password", Description = "User Password", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Password { get; set; }
    }

    [Route("/github/sshkey", "POST", Summary = "Add a key for the current user", Notes = "User is the current user")]
    public class AddGithubSSHKeyToCurrentUser : IReturn<bool> {}

    [Route("/github/sshkey", "DELETE", Summary = "Add a key for the current user", Notes = "User is the current user")]
    public class DeleteSSHKeyOfCurrentUser : IReturn<bool> {}


    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// User.
    /// </summary>
    public class WebGithubProfile {

        [ApiMember(Name = "GithubName", Description = "Github username", ParameterType = "query", DataType = "String", IsRequired = true)]
        public String GithubName { get; set; }

        [ApiMember(Name = "HasSSHKey", Description = "Has User a SSH key on github", ParameterType = "query", DataType = "bool", IsRequired = true)]
        public bool HasSSHKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.Metadata.Model.User"/> class.
        /// </summary>
        public WebGithubProfile() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.WebService.Model.User"/> class.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public WebGithubProfile(GithubProfile entity) {
            this.GithubName = entity.GithubName;
        }

        /// <summary>
        /// Tos the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="context">Context.</param>
        public GithubProfile ToEntity(IfyContext context, int usrid){
            GithubProfile user;
            try{
                user = GithubProfile.FromId(context, usrid);
            }catch(Exception){
                user = new GithubProfile(context, usrid);
            }
                       
            user.GithubName = this.GithubName;
            return user;
        }
            
    }
}

