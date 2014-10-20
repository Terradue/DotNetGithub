﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Terradue.Github.Reponse {

    [DataContract]
    public class GithubReleaseResponse {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string tag_name { get; set; }
        [DataMember]
        public GithubUserResponse author { get; set; }
        [DataMember]
        public bool prerelease { get; set; }
    }



}
