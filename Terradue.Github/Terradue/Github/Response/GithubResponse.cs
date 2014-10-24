using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github.Response {

    [DataContract]
    public class GithubTokenResponse {
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
    public class GithubAccessTokenResponse {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string scope { get; set; }
        [DataMember]
        public string token_type { get; set; }
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

