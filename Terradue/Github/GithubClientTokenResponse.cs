using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github {

    [DataContract]
    public class GithubClientTokenResponse {
        [DataMember]
        public GithubClientResponse app { get; set; }
        [DataMember]
        public DateTime created_at { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string note { get; set; }
        [DataMember]
        public string note_url { get; set; }
        [DataMember]
        public List<string> scopes { get; set; }
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public DateTime updated_at { get; set; }
        [DataMember]
        public string url { get; set; }
    }


    [DataContract]
    public class GithubClientResponse {
        [DataMember]
        public string client_id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string url { get; set; }
    }




}

