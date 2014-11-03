using System;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace Kinect.ScoreAPI
{

    [DataContract]
    public class ScoreAPIResponse
    {
        [DataMember(Name = "ErrCode")]
        public int ErrCode { get; set; }

        [DataMember(Name = "ErrMsg")]
        public string ErrMsg { get; set; }

        [DataMember(Name = "ScoreSet")]
        public Score[] ScoreSet { get; set; }

    }

    [DataContract]
    public class Score
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Value")]
        public int Value { get; set; }
    }

}
