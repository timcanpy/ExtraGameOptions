﻿using System;
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
        private static List<string> apiUrls = new List<string> { "http://fireprotrackingapi.us-east-1.elasticbeanstalk.com/api/FireProTracking/" };
        private static HttpClient client = new HttpClient();
        private static string configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.txt");
        private static string folderName;

        static async Task Main(string[] args)
        {
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (string line in lines)
                {
                    if (line.Split('~')[0].Equals("datapath"))
                    {
                        folderName = line.Split('~')[1];
                    }
                }
            }
            else
            {
                string filePath = "";
                while (!Directory.Exists(filePath))
                {
                    Console.WriteLine("Enter the path to your FirePro wrestling directory (example - C:\\Program Files(x86)\\Steam\\SteamApps\\common\\Fire Prowrestling World");
                    filePath = Console.ReadLine();
                }

                //File.Create(configPath);
                using (StreamWriter sw = File.AppendText(configPath))
                {
                    sw.WriteLine("datapath~" + filePath);
                }
                folderName = filePath;
            }

            folderName = Path.Combine(new string[] { folderName, "EGOData" });
            Console.Clear();
            Console.WriteLine("Welcome to the Fire Pro API Client! This program is meant as a tool for transfering data to various Web APIs related to Fire Pro Wrestling World.");
            string option = GetMenuAction();

            switch (option)
            {
                case "Fire Pro Tracking API":
                    client.BaseAddress = new Uri(apiUrls[0]);
                    await ProcessFPTOptionsAsync();
                    break;
            }

            Console.WriteLine("Action has been completed successfully!");
            Console.ReadKey();

        }

        private static async Task ProcessFPTOptionsAsync()
        {
            Console.Clear();
            menuOptions = new List<String> { "Upload Data", "Delete Existing Data" };

            string option = GetMenuAction();

            switch (option)
            {
                case "Upload Data":
                    await SendWarDataAsync();
                    break;
                case "Delete Existing Data":
                    await RemoveWarDataAsync();
                    break;
            }
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

        private static async Task<bool> RemoveWarDataAsync()
        {
            bool success = true;

            string promotionFile = "Promotions.json";
            string wrestlerFile = "Employees.json";
            string titleFile = "Titles.json";
            if (File.Exists(Path.Combine(folderName, promotionFile)))
            {
                //We only need one record
                var promotionData = File.ReadAllLines(Path.Combine(folderName, promotionFile));
                if (promotionData.Length > 0)
                {
                    Promotion promotion;
                    try
                    {
                        promotion = JsonConvert.DeserializeObject<Promotion>(promotionData[0]);
                    }
                    catch
                    {
                        promotion = null;
                    }

                    if (promotion != null)
                    {
                        await RemoveDataAsync(promotion.OwnerID);
                    }

                    return true;
                }

            }
            if (File.Exists(Path.Combine(folderName, wrestlerFile)))
            {
                //We only need one record
                var wrestlerData = File.ReadAllLines(Path.Combine(folderName, wrestlerFile));
                if (wrestlerData.Length > 0)
                {
                    Wrestler wrestler;
                    try
                    {
                        wrestler = JsonConvert.DeserializeObject<Wrestler>(wrestlerData[0]);
                    }
                    catch
                    {
                        wrestler = null;
                    }

                    if (wrestler != null)
                    {
                        await RemoveDataAsync(wrestler.OwnerID);
                    }

                    return true;
                }
            }
            if (File.Exists(Path.Combine(folderName, titleFile)))
            {
                var titleData = File.ReadAllLines(Path.Combine(folderName, titleFile));
                Title title;
                if(titleData.Length > 0)
                {
                    try
                    {
                        title = JsonConvert.DeserializeObject<Title>(titleData[0]);
                    }
                    catch
                    {
                        title = null;
                    }

                    if(title != null)
                    {
                        await RemoveDataAsync(title.OwnerID);
                    }
                }
            }

            return success;
        }
        private static async Task<bool> SendWarDataAsync()
        {
            bool success = true;

            string promotionFile = "Promotions.json";
            string wrestlerFile = "Employees.json";
            string titleFile = "Titles.json";

            //Creating the necessary objects for API transfer
            if (File.Exists(Path.Combine(folderName, promotionFile)))
            {
                var promotionData = File.ReadAllLines(Path.Combine(folderName, promotionFile));
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

            if (File.Exists(Path.Combine(folderName, wrestlerFile)))
            {
                var wrestlerData = File.ReadAllLines(Path.Combine(folderName, wrestlerFile));
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

            if (File.Exists(Path.Combine(folderName, titleFile)))
            {
                var titleData = File.ReadAllLines(Path.Combine(folderName, titleFile));
                List<Title> titles = new List<Title>();

                Console.WriteLine("titles");
                foreach (string line in titleData)
                {
                    try
                    {
                        Title title = JsonConvert.DeserializeObject<Title>(line);
                        if (title.CurrentChamp.Equals(String.Empty))
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

        private static async Task<HttpResponseMessage> RemoveDataAsync(string ownerID)
        {
            return await client.DeleteAsync("RemoveRecords?ownerID=" + ownerID);
        }

        private static async Task<HttpResponseMessage> SendPromotionsAsync(List<Promotion> promotions)
        {
            return await client.PostAsJsonAsync("AddPromotions", promotions);
        }

        private static async Task<HttpResponseMessage> SendWrestlersAsync(List<Wrestler> wrestlers)
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
