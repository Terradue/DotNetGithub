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
    public class GetNewGithubToken : IReturn<WebGithubProfile> {
        [ApiMember(Name = "code", Description = "User code", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Code { get; set; }
    }

    [Route("/github/sshkey", "POST", Summary = "Add a key for the current user", Notes = "User is the current user")]
    public class AddGithubSSHKeyToCurrentUser : IReturn<WebGithubProfile> {
        [ApiMember(Name = "sshkey", Description = "User code", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string SshKey { get; set; }
    }

    [Route("/github/sshkey", "DELETE", Summary = "Add a key for the current user", Notes = "User is the current user")]
    public class DeleteSSHKeyOfCurrentUser : IReturn<WebGithubProfile> {}

    [Route("/github/user/current", "GET", Summary = "GET user github information", Notes = "User is the current user")]
    public class GetGithubUser : IReturn<WebGithubProfile> {}

    [Route("/github/user", "PUT", Summary = "Update github information about current user", Notes = "User is the current user")]
    public class UpdateGithubUser : WebGithubProfile, IReturn<WebGithubProfile> {}

    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// User.
    /// </summary>
    public class WebGithubProfile : WebEntity {

        [ApiMember(Name = "HasSSHKey", Description = "Has User a SSH key on github", ParameterType = "query", DataType = "bool", IsRequired = true)]
        public bool HasSSHKey { get; set; }

        [ApiMember(Name = "CertPub", Description = "User public cert ssh key", ParameterType = "query", DataType = "String", IsRequired = true)]
        public String CertPub { get; set; }

        [ApiMember(Name = "Avatar", Description = "Github avatar url", ParameterType = "query", DataType = "String", IsRequired = true)]
        public String Avatar { get; set; }

        [ApiMember(Name = "Token", Description = "Token Description", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.Metadata.Model.User"/> class.
        /// </summary>
        public WebGithubProfile() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.WebService.Model.User"/> class.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public WebGithubProfile(GithubProfile entity) : base(entity) {
            this.Avatar = entity.Avatar;
            this.CertPub = entity.PublicSSHKey;
            this.HasSSHKey = entity.HasSSHKey;
            this.Token = entity.Token;
        }

        /// <summary>
        /// Tos the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="context">Context.</param>
        public GithubProfile ToEntity(IfyContext context, GithubProfile input){
            GithubProfile user = (input == null ? new GithubProfile(context, this.Id) : input);
            user.Name = this.Name;
            return user;
        }
            
    }
}

