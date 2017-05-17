using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace HttpStstusCodeScraper
{
    class Program
    {
        public static List<string> URLs= new List<string> ();
        public static string Outfile = "OUTPUT.csv";
        static void Main(string[] args)
        {
            Console.WriteLine("Input File path that has URLs:");
            string path=Console.ReadLine();
            Console.WriteLine("Read In started.");
            ReadinURL(path);
            Console.WriteLine("Read In done.");
            Console.WriteLine("Let the scrape-in BEGIN!!!");
            for (int x=0;x<URLs.Count;++x)
            {
                Scrape(URLs.ElementAt(x));
            }
        }

        public static void ReadinURL(string Filepath)
        {
              string line;
                    if (!File.Exists(Filepath))
                    {
                        Console.WriteLine("Your file path sucks error :)");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                    using (var reader = new StreamReader(Filepath, Encoding.UTF8))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            URLs.Add(line);
                        }
                        reader.Close();
                    }
        }

        public static string Scrape(string Url)
        {
            string status="UNKNOWN";
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://"+Url);
            httpReq.AllowAutoRedirect = true;
            httpReq.Timeout = 400;
            try
            {
                HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();
                int code = Convert.ToInt32(httpRes.StatusCode);
                if (code >= 200 && code < 300)
                {
                    status = code.ToString();
                }
                else
                {
                    status = code.ToString(); ;
                }
                // Close the response.
                httpRes.Close();
                File.AppendAllText(Outfile, Url + "," + status);  
            }
            catch
            {
                status = "LINK LIKELY DOWN";
                File.AppendAllText(Outfile, Url + "," + status+"\n");  
            }
            Console.WriteLine(Url + "Status: " + status);
            return status;
        }
    }
}
