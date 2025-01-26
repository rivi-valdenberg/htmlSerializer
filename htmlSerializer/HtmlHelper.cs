using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel;

namespace Html_Serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        private HtmlHelper()
        {
            try
            {
                string allTagsJson = File.ReadAllText("JSON Files/HtmlTags.json");
                string selfClosingTagsJson = File.ReadAllText("JSON Files/HtmlVoidTags.json");

                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
                SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tags: {ex.Message}");
            }
        }


    }
}
