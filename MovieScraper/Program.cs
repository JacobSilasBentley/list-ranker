using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieScraper
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            HttpResponseMessage response = await client.GetAsync("https://www.imdb.com/list/ls070880401/?sort=moviemeter,asc&st_dt=&mode=detail&page=1");
            string body = await response.Content.ReadAsStringAsync();
            Regex regex = new Regex(@"<h3.*>.*<\/h3>");
            MatchCollection matches = regex.Matches(body);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}",
                                  groups["word"].Value,
                                  groups[0].Index,
                                  groups[1].Index);
            }


        }
    }
}
