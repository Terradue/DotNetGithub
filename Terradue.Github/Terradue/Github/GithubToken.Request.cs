using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github {

    [DataContract]
    public class GithubTokenRequest {
        [DataMember]
        public string client_id { get; set; }
        [DataMember]
        public string client_secret { get; set; }
        [DataMember]
        public string code { get; set; }
    }



}

