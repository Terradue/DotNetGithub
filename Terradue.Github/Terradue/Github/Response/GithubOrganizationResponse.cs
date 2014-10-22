using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github.Reponse {

    [DataContract]
    public class GithubRepositoryResponse {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string full_name { get; set; }
        [DataMember]
        public GithubUserResponse owner { get; set; }
        [DataMember(Name="private")]
        public bool Private { get; set; }
    }



}

