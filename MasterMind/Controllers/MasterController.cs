using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MasterMind.Controllers
{
    public class ResponseData
    {
        public string Original { get; set; }
        public string Attempt { get; set; }
        public int Present { get; set; }
        public int CorrectPos { get; set; }
        public int WrongPos { get; set; }
        public string status { get; set; }
    }
    public class MasterController : ApiController
    {
        // GET api/Master/GetNumber
        [HttpGet]
        public string GetNumber()
        {
            string number = GetRandomNumber().ToString();
            return number;
            //return new string[] {GetRandomNumber().ToString() };
        }

        public int GetRandomNumber()
        {
            int result = 4912;
            bool res = true;
            while (res)
            {
                Random rand = new Random();
                int num = rand.Next(1000, 9999);
                if (check(num))
                {
                    result = num;
                    res = false;
                }
            }
            return result;
        }

        public bool check(int n)
        {
            bool res = false;
            int[] arr = new int[10];
            while(n > 0)
            {
                int d = n % 10;
                arr[d]++;
                if (arr[d] == 2 || arr[0] == 1)
                {
                    res = false;
                    break;
                }
                else
                {
                    res = true;
                }
                n = n / 10;
            }
            return res;
        }


        //POST::: api/Master/GetResults
        [HttpPost]
        public IHttpActionResult GetResults([FromBody] ResponseData obj)
        {
            try
            {
                if(obj.Original == obj.Attempt)
                {
                    obj.status = "true";
                    return Json<ResponseData>(obj);
                }
                else
                {
                    ResponseData response = new ResponseData();
                    response = getResults(obj);
                    return Json<ResponseData>(response);
                }

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        
        public ResponseData getResults(ResponseData obj)
        {
            ResponseData result = new ResponseData();
            result.status = "false";
            result.Original = obj.Original;
            result.Attempt = obj.Attempt;
            //int[] present = new int[10];   //present with correct pos = 2, present with wrong pos = 1
            int present = 0, correct_pos = 0, wrong_pos = 0;
            foreach(char ch in result.Attempt)
            {
                int index_Original = result.Original.IndexOf(ch);
                if(index_Original >= 0)
                {
                    present += 1;
                    int index_Attempt = result.Attempt.IndexOf(ch);   //int index1 = Convert.ToInt32(sl[1]) - Convert.ToInt32('0');

                    if(index_Original == index_Attempt)
                    {
                        correct_pos += 1;
                    }
                    else
                    {
                        wrong_pos += 1;
                    }
                }
            }
            result.Present = present;
            result.WrongPos = wrong_pos;
            result.CorrectPos = correct_pos;
            
            return result;
        }

        public string url = "http://103.118.115.126:8080/eTmcsWeb/eChallanPDF/SPD20221209183221-229";

        [HttpGet]
        public string GetHTML()
        {
            string response = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    response = reader.ReadToEnd();
                }
            }
            return response; 
        }
    }
}