using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github {

    [DataContract]
    public class GithubKeyRequest {
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string title { get; set; }
    }

}

