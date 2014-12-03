using System;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Kinect.ScoreAPI
{

    public class ScoreAPI
    {
        //UPDATE THIS URL AS NECESSARY
        private const string URL = "http://luna.mines.edu/team07/ccopper/scoreAPI.php";

        //This is just a simple test code for the API.
        //To run change the project to a console app and run.
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

        //Code is more or less identical for both API calls so just this one will be commented.
        public static ScoreAPIResponse RequestScores(string GameName, int Count, int StartPos)
        {
            //This is the parameter string to pass the arguments to the endpoint
            string URLParam = String.Format("?methodName=requestScores&gameName={0}&count={1}&startPos={2}", GameName, Count, StartPos);

            //Connections may fail at several points during the call.
            //Catch any failure and return a valid ScoreAPIResponse
            try
            {
                //Create Reques with a 1.5 second time out.  Default timeout is much to long and the calls do block
                HttpWebRequest request = WebRequest.Create(URL + URLParam) as HttpWebRequest;
                request.Timeout = 1500;
                //Make the request and get the HTTP Respons from the server
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //If the server returned anything other than HTTP 200(Ok)
                    //Report the HTTP error code and a message in the ScoreAPI Response
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        ScoreAPIResponse sr = new ScoreAPIResponse();
                        sr.ErrCode = (int)response.StatusCode;
                        sr.ErrMsg = response.StatusDescription;
                        return sr;
                    }
                    //Using the reponse convert th JSON returned to a ScoreAPIResponse object and return it
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ScoreAPIResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    ScoreAPIResponse jsonResponse = objResponse as ScoreAPIResponse;
                    return jsonResponse;
                }
            //Connection failures, JSON converstion errors and anything else is caught here and returned with a status of -1
            // Along with a message of the error
            } catch(Exception e)
            {
                ScoreAPIResponse sr = new ScoreAPIResponse();
                sr.ErrCode = -1;
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
                sr.ErrCode = -1;
                sr.ErrMsg = e.Message;
                return sr;
            }
        }
    
    
    }
}
