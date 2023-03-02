using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace Scraper {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine(@" 
                   __  ______  __       _____                                      
                  / / / / __ \/ /      / ___/______________ _____  ___  _____
                 / / / / /_/ / /       \__ \/ ___/ ___/ __ `/ __ \/ _ \/ ___/
                / /_/ / _, _/ /___    ___/ / /__/ /  / /_/ / /_/ /  __/ /    
                \____/_/ |_/_____/   /____/\___/_/   \__,_/ .___/\___/_/     
                                                         /_/                 
            ");

            do {
                try {
                    Console.Write("\nEnter URL which you want to scrape(eg.google.com): ");
                    var domain = Console.ReadLine();
                    List<string> links = ParseLinks("https://" + domain);
                    foreach (var link in links)
                        Console.WriteLine("- " + link);

                    Console.Write("Press 'Q' to exit the process... or any other key to scrape again");
                }
                catch(Exception ex) {
                    Console.WriteLine("proper domain name was not provided." + ex.Message);
                }
                
            } while (Console.ReadKey().Key != ConsoleKey.Q);
        }

        static List<string> ParseLinks(string urlToCrawl) {

            WebClient webClient = new WebClient();

            byte[] data = webClient.DownloadData(urlToCrawl);
            string download = Encoding.ASCII.GetString(data);

            HashSet<string> list = new HashSet<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(download);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            foreach (var n in nodes) {
                string href = n.Attributes["href"].Value;
                list.Add(GetAbsoluteURL(urlToCrawl, href));
            }
            return list.ToList();
        }


        static string GetAbsoluteURL(string baseURL, string url) {
            var URL = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!URL.IsAbsoluteUri)
                URL = new Uri(new Uri(baseURL), URL);
            return URL.ToString();
        }
    }
}
