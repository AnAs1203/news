using HtmlAgilityPack;
using System;
using System.Net.Http;

namespace WebScraping;

public class Program
{
    
    public static void Main(string[] args)
    {
        // Send get request to aftonbladet.se
        string url = "https://www.aftonbladet.se/";
        var httpClient = new HttpClient();
        var html = httpClient.GetStringAsync(url).Result;
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        
        // Get the headline
        var headlineElements = htmlDocument.DocumentNode.SelectNodes("//h2[@class='_title_iqu05_198']");
        
        // Get the article link
        var articleElements = htmlDocument.DocumentNode.SelectNodes("//a[@class='color-text-primary _link_iqu05_98']");
        
        // Print out
        for (int i = 0; i < headlineElements.Count; i++)
        {
            string articleURL = articleElements[i].Attributes["href"].Value;
            
            if (articleURL.Contains("tipsa") || articleURL.Contains("pussel"))
                continue;
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($">>>> HEADLINE: {headlineElements[i].Attributes["aria-label"].Value}");
            Console.ResetColor();
            Console.WriteLine("\n----------------------------------"); 
            
            var HTML = httpClient.GetStringAsync(articleURL).Result;
            var HTMLDocument = new HtmlDocument();
            HTMLDocument.LoadHtml(HTML);

            if (!articleURL.Contains("video"))
            {
                var articleDescriptions = HTMLDocument.DocumentNode.SelectNodes("//p[@class='hyperion-css-1shl3oz']");
                
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var description in articleDescriptions)
                {
                    Console.Write($">>> {description.InnerText} ");
                }
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine($">> More info: {articleURL}");
            Console.WriteLine("\n");
        }
    }
}