using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Parse(string query)
        {
            string[] parts = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = null;
            Selector current = null;

            foreach (var part in parts)
            {
                Selector selector = new Selector();
                string s = part;

                int tagEndIndex = s.IndexOfAny(new[] { '#', '.' });
                if (tagEndIndex == -1)
                {

                    selector.TagName = s;
                    s = string.Empty;
                }
                else if (tagEndIndex > 0)
                {
                    selector.TagName = s.Substring(0, tagEndIndex);
                    s = s.Substring(tagEndIndex);
                }


                int idIndex = s.IndexOf('#');
                if (idIndex != -1)
                {
                    int idEndIndex = s.IndexOf('.', idIndex);
                    if (idEndIndex == -1) idEndIndex = s.Length;

                    selector.Id = s.Substring(idIndex + 1, idEndIndex - idIndex - 1);
                    s = s.Remove(idIndex, idEndIndex - idIndex);
                }

                if (!string.IsNullOrEmpty(s))
                {
                    selector.Classes = s.Split('.')
                                                .Where(c => !string.IsNullOrEmpty(c))
                                                .ToList();
                }

                if (root == null)
                {
                    root = selector;
                }
                else
                {
                    current.Child = selector;
                    selector.Parent = current;
                }

                current = selector;
            }
            return root;
        }

    }


}
