using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace htmlSerializer
{
    internal class HtmlHelper
    {
        public string[] AllTags { get; private set; }
        public string[] SelfClosingTags { get; private set; }

        public HtmlHelper()
        {
            // Define file paths explicitly
            string allTagsFilePath = "\"C:\\Users\\user1\\Downloads\\JSON Files\\JSON Files\\HtmlTags.json\"";
            string selfClosingTagsFilePath = "\"C:\\Users\\user1\\Downloads\\JSON Files\\JSON Files\\HtmlVoidTags.json\"";

            // Load all tags from the JSON file
            string allTagsJson = File.ReadAllText(allTagsFilePath);
            AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);

            // Load self-closing tags from the JSON file
            string selfClosingTagsJson = File.ReadAllText(selfClosingTagsFilePath);
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
        }
    }
}
