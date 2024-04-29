
using System.Reflection;

namespace Console_Html_Parser.StaticTools
{
    public static class Tools
    {
        public static string FindValue(string html, string firstAnchor, string secondAnchor, string borderSymb)
        {
            var strStart = html.IndexOf(firstAnchor);
            strStart = html.IndexOf(secondAnchor, strStart) + secondAnchor.Length;

            var strEnd = html.IndexOf(borderSymb, strStart);

            return html.Substring(strStart, strEnd - strStart);
        }

        public static void ShowItem<T>(T item)
        {
            Type type = item.GetType();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;

                var propertyValue = property.GetValue(item);

                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine($"{propertyName}: {propertyValue}");
            }
        }
    }
}
