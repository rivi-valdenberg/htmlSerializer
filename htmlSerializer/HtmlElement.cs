using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class HtmlElement
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public List<string>? Classes { get; set; }
        public string? InnerHtml { get; set; }
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new Dictionary<string, string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
            InnerHtml = "";
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);

            while (q.Count > 0)
            {
                var element = q.Dequeue();
                yield return element;//מוסיף לרשימה את אלמנט

                foreach (var child in element.Children)
                {
                    q.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
        public IEnumerable<HtmlElement> QuerySelector(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();
            Search(this, selector, results);
            return results;
        }

        private void Search(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            var descendants = element.Descendants();
            var matchingElements = descendants.Where(el =>
                                   (selector.TagName == null || el.Name == selector.TagName) &&
                                   (selector.Id == null || (el.Id != null && el.Id == selector.Id)) &&
                                   (selector.Classes.Count == 0 || selector.Classes.All(cls => el.Classes.Contains(cls))));
            if (selector.Child != null)
            {
                matchingElements.ToList().ForEach(match => Search(match, selector.Child, results));
                //foreach (var match in matchingElements)
                //{
                //    Search(match, selector.Child, results);
                //}
            }
            else//הגענו לבן האחרון ולכן אנחנו צריכים להחזיר את הרשימה של המתאימים 
            {
                // תנאי עצירה: אם אין סלקטור בן
                matchingElements.ToList().ForEach(match => results.Add(match));
                //foreach (var match in matchingElements)
                //{
                //    results.Add(match);
                //}
            }
        }

    }
}
