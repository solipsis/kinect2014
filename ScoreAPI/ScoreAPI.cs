using System;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Kinect.ScoreAPI
{
    public class DataObject
    {
        public string Name { get; set; }
    }

    public class ScoreAPI
    {
        private const string URL = "http://192.168.56.101/CSM_IS/scoreAPI.php";

        static void Main(string[] args)
        {
            ScoreAPIResponse sr = SubmitScore("DummyGame", "Frank Smith", 500);
            if(sr.ErrCode !=0)
            {
                Console.WriteLine("Submit Failed");
            }
            sr = RequestScores("DummyGame", 5,0);
            Console.WriteLine("{0} {1}", sr.ErrCode, sr.ErrMsg);
            if (sr.ErrCode == 0)
            {
                foreach (Score s in sr.ScoreSet)
                {
                    Console.WriteLine("{0} {1}", s.Name, s.Value);
                }
            }
            Console.ReadLine();
        }


        public static ScoreAPIResponse RequestScores(string GameName, int Count, int StartPos)
        {
            string URLParam = String.Format("?methodName=requestScores&gameName={0}&count={1}&startPos={2}", GameName, Count, StartPos);

            try
            {
                HttpWebRequest request = WebRequest.Create(URL + URLParam) as HttpWebRequest;
                request.Timeout = 1500;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ScoreAPIResponse sr = new ScoreAPIResponse();
                        sr.ErrCode = (int)response.StatusCode;
                        sr.ErrMsg = response.StatusDescription;
                        return sr;
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ScoreAPIResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    ScoreAPIResponse jsonResponse = objResponse as ScoreAPIResponse;
                    return jsonResponse;
                }
            } catch(Exception e)
            {
                ScoreAPIResponse sr = new ScoreAPIResponse();
                sr.ErrCode = 1;
                sr.ErrMsg = e.Message;
                return sr;
            }
        }

        public static ScoreAPIResponse SubmitScore(string GameName, string Name, int Value)
        {
            string URLParam = String.Format("?methodName=submitScore&gameName={0}&userName={1}&value={2}", GameName, Name, Value);

            try
            {
                HttpWebRequest request = WebRequest.Create(URL + URLParam) as HttpWebRequest;
                request.Timeout = 1500;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ScoreAPIResponse sr = new ScoreAPIResponse();
                        sr.ErrCode = (int)response.StatusCode;
                        sr.ErrMsg = response.StatusDescription;
                        return sr;
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ScoreAPIResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    ScoreAPIResponse jsonResponse = objResponse as ScoreAPIResponse;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                ScoreAPIResponse sr = new ScoreAPIResponse();
                sr.ErrCode = 1;
                sr.ErrMsg = e.Message;
                return sr;
            }
        }
    
    
    }
}
