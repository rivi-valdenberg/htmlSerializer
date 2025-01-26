using Html_Serializer;
using htmlSerializer;
using System.Text.RegularExpressions;
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
static string[] SeparateHtmlTags(string html)
{
    var cleanHtml = new Regex("\\s+").Replace(html, " ");
    var matches = new Regex("<(.*?)>").Split(cleanHtml);
    return matches.Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(match => match.Trim()).ToArray();
}

static HtmlElement BuildHtmlTree(string[] htmlLines)
{
    HtmlElement root = new HtmlElement() { Name = "html" }, current = root;

    for (int i = 0; i < htmlLines.Length; i++)
    {
        var firstWord = htmlLines[i].Split()[0];
        if (firstWord == "/html")
            return root;
        else if (firstWord.StartsWith('/'))
            current = current.Parent;

        else if (HtmlHelper.Instnace.HtmlTags.Contains(firstWord)
            || HtmlHelper.Instnace.HtmlVoidTags.Contains(firstWord))
        {
            var newElement = new HtmlElement() { Name = firstWord, Parent = current };
            var tagEnoughFirstWord = htmlLines[i].Substring(firstWord.Length);
            var attributes = Regex.Matches(tagEnoughFirstWord, "([^\\s]?)=\"(.?)\"").ToDictionary(
                         match => match.Groups[1].Value,
                         match => match.Groups[2].Value);

            newElement.Attributes = attributes;
            foreach (var attribute in attributes)
            {
                if (attribute.Key == "class")
                    newElement.Classes = new Regex("\\s+").Replace(attribute.Value, " ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<string?>();
                if (attribute.Key == "id")
                    newElement.Id = attribute.Value;
            }
            current.Children.Add(newElement);
            if (!HtmlHelper.Instnace.HtmlVoidTags.Contains(firstWord) && htmlLines[i][htmlLines[i].Length - 1] != '/')
                current = newElement;
        }
        else
            current.InnerHtml += htmlLines[i];
    }
    return root;
}