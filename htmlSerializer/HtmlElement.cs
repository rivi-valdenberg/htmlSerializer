using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            var queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                yield return element;

                foreach (var child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }

        }

        public IEnumerable<HtmlElement> FindElementsBySelector(Selector selector)
        {
            var results = new HashSet<HtmlElement>();
            FindElementsBySelectorRecursive(this, selector, results);
            return results;
        }

        private void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            if (selector == null || element == null)
                return;

            var dece = element.Descendants();
            foreach (var descendant in dece)
            {
                if (MatchesSelector(descendant, selector))
                {
                    if (selector.Child == null)
                    {
                        results.Add(descendant);

                    }
                    FindElementsBySelectorRecursive(descendant, selector.Child, results);

                }
            }
        }

        private bool MatchesSelector(HtmlElement element, Selector selector)
        {
            var s = "\"" + selector.Id + "\"";
            if (selector.Id != "" && !s.Equals(element.Id))
                return false;
            if (selector.TagName != element.Name)
                return false;

            foreach (var c in selector.Classes)
                if (element.Classes.Count > 0 && !element.Classes.Contains(c))
                    return false;
            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Id: {Id}");
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine("Attributes:");
            foreach (var attribute in Attributes)
            {
                stringBuilder.AppendLine($"   {attribute.Key}: {attribute.Value}");
            }
            stringBuilder.AppendLine("Classes:");
            foreach (var className in Classes)
            {
                stringBuilder.AppendLine($"   {className}");
            }
            stringBuilder.AppendLine($"InnerHtml: {InnerHtml}");
            stringBuilder.AppendLine($"Parent: {Parent?.Id ?? Parent?.Name ?? "null"}"); // Parent might be null
            stringBuilder.AppendLine("Children:");
            foreach (var child in Children)
            {
                stringBuilder.AppendLine($"   {child.Name ?? child.Id}");
            }

            return stringBuilder.ToString();
        }
    }
}


