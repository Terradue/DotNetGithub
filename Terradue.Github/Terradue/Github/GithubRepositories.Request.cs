using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github {

    [DataContract]
    public class GithubRepositoryRequest {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember(Name="private")]
        public bool Private { get; set; }
    }



}

