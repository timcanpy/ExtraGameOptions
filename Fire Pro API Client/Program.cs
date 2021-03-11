using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FirePro_API.Models;
using Newtonsoft.Json;

namespace Fire_Pro_API_Client
{
    class Program
    {
        private static List<string> menuOptions = new List<string> { "Fire Pro Tracking API" };
        private static List<string> apiUrls = new List<string> { "https://localhost:44340/api/FireProTracking/" };
        private static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Fire Pro API Client! This program is meant as a tool for transfering data to various Web APIs related to Fire Pro Wrestling World.");
            string option = GetMenuAction();

            switch (option)
            {
                case "Fire Pro Tracking API":
                    client.BaseAddress = new Uri(apiUrls[0]);
                    await SendWarDataAsync();
                    break;
            }

            Console.WriteLine("Action has been completed successfully!");
            Console.ReadKey();

        }

        private static string GetMenuAction()
        {
            string result = "";
            Console.WriteLine("Please select an option from the following menu items:");
            for (int i = 0; i < menuOptions.Count; i++)
            {
                Console.WriteLine(i + ". " + menuOptions[i]);
            }

            result = Console.ReadLine();

            while (!ValidateMenuAction(result))
            {
                result = Console.ReadLine();
            }

            return menuOptions[Int32.Parse(result)];
        }

        private static bool ValidateMenuAction(string action)
        {

            if (!int.TryParse(action, out int value))
            {
                Console.WriteLine("Please enter a numeric value.");
                return false;
            }

            if (int.Parse(action) > menuOptions.Count - 1)
            {
                Console.WriteLine("Please enter a valid option.");
                return false;
            }

            return true;
        }

        private static async Task<bool> SendWarDataAsync()
        {
            bool success = true;

            string folderName = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Fire Prowrestling World\\EGOData\\";
            string promotionFile = "Promotions.json";
            string wrestlerFile = "Employees.json";
            string titleFile = "Titles.json";


            //Creating the necessary objects for API transfer
            if (File.Exists(String.Concat(folderName, promotionFile)))
            {
                var promotionData = File.ReadAllLines(String.Concat(folderName, promotionFile));
                List<Promotion> promotions = new List<Promotion>();

                Console.WriteLine("Promotions");
                foreach (string line in promotionData)
                {
                    try
                    {
                        Promotion promotion = JsonConvert.DeserializeObject<Promotion>(line);
                        promotions.Add(promotion);
                        promotion.Name = RemoveDiacritics(promotion.Name);
                        Console.WriteLine("Adding " + promotion.Name);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine("Non-fatal error with promotion conversion: " + ex);
                    }
                }

                try
                {
                    var response = await SendPromotionsAsync(promotions);
                    Console.WriteLine("Response: " + response);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error contacting AddPromotions: " + ex);
                    success = false;
                }
            }

            if (File.Exists(String.Concat(folderName, wrestlerFile)))
            {
                var wrestlerData = File.ReadAllLines(String.Concat(folderName, wrestlerFile));
                List<Wrestler> wrestlers = new List<Wrestler>();

                Console.WriteLine("wrestlers");
                foreach (string line in wrestlerData)
                {
                    try
                    {
                        Wrestler wrestler = JsonConvert.DeserializeObject<Wrestler>(line);
                        wrestlers.Add(wrestler);
                        wrestler.Name = RemoveDiacritics(wrestler.Name);
                        Console.WriteLine("Adding " + wrestler.Name);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine("Non-fatal error with wrestler conversion: " + ex);
                    }
                }

                try
                {
                    HttpResponseMessage response = await SendWrestlersAsync(wrestlers);
                    Console.WriteLine("Response: " + response);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error contacting AddWrestlers: " + ex);
                    success = false;
                }
            }

            if (File.Exists(String.Concat(folderName, titleFile)))
            {
                var titleData = File.ReadAllLines(String.Concat(folderName, titleFile));
                List<Title> titles = new List<Title>();

                Console.WriteLine("titles");
                foreach (string line in titleData)
                {
                    try
                    {
                        Title title = JsonConvert.DeserializeObject<Title>(line);
                        if(title.CurrentChamp.Equals(String.Empty))
                        {
                            title.CurrentChamp = "None";
                        }
                        titles.Add(title);
                        title.CurrentChamp = RemoveDiacritics(title.CurrentChamp);
                        title.Name = RemoveDiacritics(title.Name);
                        Console.WriteLine("Adding " + title.Name);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine("Non-fatal error with title conversion: " + ex);
                    }
                }

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("AddTitles", titles);
                    Console.WriteLine("Response: " + response);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error contacting AddTitles: " + ex);
                    success = false;
                }
            }

            return success;
        }

        private static async Task<HttpResponseMessage> SendPromotionsAsync(List<Promotion> promotions)
        {
            return await client.PostAsJsonAsync("AddPromotions", promotions);
        }

        private static async Task<HttpResponseMessage> SendWrestlersAsync (List<Wrestler> wrestlers)
        {
           return await client.PostAsJsonAsync("AddWrestlers", wrestlers);
        }

        private static async Task<HttpResponseMessage> SendTitlesAsync(List<Title> titles)
        {
            return await client.PostAsJsonAsync("AddTitles", titles);
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
