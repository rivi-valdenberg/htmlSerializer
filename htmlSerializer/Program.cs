using Html_Serializer;
using System.Text.Json;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

static HtmlElement Parse(IEnumerable<string> htmlLines)
{
    HtmlElement root = new HtmlElement();
    HtmlElement current = root;
    foreach (var l in htmlLines)
    {
        string[] line = l.Split(' ');
        if (!line[0].Equals("/html"))
        {
            if (line[0].StartsWith("/"))
            {
                if (current == null)
                {
                    Console.WriteLine("Warning: current is null when processing closing tag.");
                    continue;
                }
                if (current.Parent == null)
                {
                    Console.WriteLine($"Warning: Parent is null for tag {line[0]}.");
                    continue;
                }
                current = current.Parent;
            }
            else if (HtmlHelper.Instance.AllTags.Contains(line[0]))
            {
                HtmlElement newElement = new HtmlElement();
                newElement.Parent = current;
                newElement.Name = line[0];
                current.Children.Add(newElement);
                if (l.IndexOf(' ') > 0)
                {
                    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(l.Substring(l.IndexOf(' '))).ToList();
                    foreach (var attribute in attributes)
                    {
                        var arr = attribute.ToString().Split('=');
                        if (arr[0].Contains("id"))
                            newElement.Id = arr[1];
                        else if (arr[0].Contains("class"))
                        {
                            newElement.Classes = arr[1].Split(" ").ToList();
                        }
                        else
                            newElement.Attributes[arr[0]] = arr[1];
                    }
                }
                if (!HtmlHelper.Instance.SelfClosingTags.Contains(line[0]))
                {
                    current = newElement;
                }
            }
            else
            {
                if (current != null)
                    current.InnerHtml = l;
            }

        }
    }
    root.Parent = null;
    return root;

}
void PrintTreeHtmlElement(HtmlElement root)
{
    if (root == null)
        return;
    Console.WriteLine(root.ToString());
    for (int i = 0; i < root.Children.Count; i++) { PrintTreeHtmlElement(root.Children[i]); }
}


static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
void Check(string s, HtmlElement dom)
{
    Selector selector = Selector.Parse(s);
    var result = dom.FindElementsBySelector(selector);
    result.ToList().ForEach(element => { Console.WriteLine(element); });
}
var html = await Load("https://hebrewbooks.org/");//loading html from website
html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);
var root = Parse(htmlLines);
PrintTreeHtmlElement(root);
