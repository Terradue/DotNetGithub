using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github {

    [DataContract]
    public class GithubKeyResponse {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string verified { get; set; }
    }

}

