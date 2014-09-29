using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceInterface;
using Terradue.Portal;
using Terradue.Github;
using Terradue.Github.Reponse;

namespace Terradue.WebService.Model {

    [Route("/github/org/{organization}/repo", "POST", Summary = "Add a repo", Notes = "User is the current user")]
    public class CreateRepository : WebGithubRepository, IReturn<WebGithubRepository> {
        [ApiMember(Name="organization", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string organization { get; set; }
    }

    [Route("/github/org/{organization}/repo", "GET", Summary = @"GET a list of repos on github", Notes = "organization is selected from its name")]
    public class GetGithubRepos : IReturn<List<WebGithubRepository>>
    {
        [ApiMember(Name="organization", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string organization { get; set; }
    }

    [Route("/github/org/{organization}/{repo}", "GET", Summary = @"GET a repo on github", Notes = "organization is selected from its name")]
    public class GetGithubRepo : IReturn<WebGithubRepository>
    {
        [ApiMember(Name="organization", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string organization { get; set; }

        [ApiMember(Name="repo", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string repo { get; set; }
    }

    [Route("/github/org/{organization}/{repo}/release", "GET", Summary = @"GET a list of releases on github", Notes = "organization is selected from its name")]
    public class GetGithubReleases : IReturn<List<string>>
    {
        [ApiMember(Name="organization", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string organization { get; set; }
        [ApiMember(Name="repo", Description = "organization", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string repo { get; set; }
    }


    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// User.
    /// </summary>
    public class WebGithubRepository {

        [ApiMember(Name = "Id", Description = "repository id", ParameterType = "query", DataType = "int", IsRequired = true)]
        public int Id { get; set; }

        [ApiMember(Name = "Name", Description = "Repository name", ParameterType = "query", DataType = "String", IsRequired = true)]
        public String Name { get; set; }

        [ApiMember(Name = "Description", Description = "Repository description", ParameterType = "query", DataType = "String", IsRequired = true)]
        public String Description { get; set; }

        [ApiMember(Name = "Private", Description = "Repository is private", ParameterType = "query", DataType = "string", IsRequired = true)]
        public bool Private { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.Metadata.Model.User"/> class.
        /// </summary>
        public WebGithubRepository() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Terradue.WebService.Model.User"/> class.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public WebGithubRepository(GithubRepositoryResponse entity) : base(entity) {
            this.Id = entity.id;
            this.Name = entity.name;
            this.Description = entity.description;
            this.Private = entity.Private;
        }

        /// <summary>
        /// Tos the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="context">Context.</param>
        public GithubRepositoryResponse ToEntity(IfyContext context){
            GithubRepositoryResponse repo = new GithubRepositoryResponse();
            repo.id = this.Id;
            repo.name = this.Name;
            repo.description = this.Description;
            repo.Private = this.Private;
            return repo;
        }
            
    }
}

