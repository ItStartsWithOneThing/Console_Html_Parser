
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Console_Html_Parser.StaticTools
{
    public static class Tools
    {
        /// <summary>
        /// Searching needed value in html page
        /// </summary>
        /// <param name="html">html page itself</param>
        /// <param name="firstAnchor">unique text anchor</param>
        /// <param name="secondAnchor">unique text before first anchor and target value</param>
        /// <param name="borderSymb">symbol that located right after target value</param>
        /// <returns></returns>
        public static string FindValue(string html, string firstAnchor, string secondAnchor, string borderSymb)
        {
            var strStart = html.IndexOf(firstAnchor);
            strStart = html.IndexOf(secondAnchor, strStart) + secondAnchor.Length;

            var strEnd = html.IndexOf(borderSymb, strStart);

            return html.Substring(strStart, strEnd - strStart);
        }

        /// <summary>
        /// Prints all property name - property value of the given object to Console using Reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        public static void ShowItem<T>(T item)
        {
            try
            {
                Type type = item.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    string propertyName = property.Name;

                    var propertyValue = property.GetValue(item);

                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{propertyName}: {propertyValue}");
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Prints all property name - property value from given collections to Console
        /// </summary>
        /// <param name="propertiesNames"></param>
        /// <param name="propertiesValues"></param>
        public static void ShowItem(List<string> propertiesNames, List<string> propertiesValues)
        {
            for(int i = 0; i < propertiesNames.Count; i++)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine($"{propertiesNames[i]}: {propertiesValues[i]}");
            }
        }

        /// <summary>
        /// Prints all given item's property names - property values from given collections to Console
        /// </summary>
        /// <param name="propertiesNames">List of items that represent's collection of their propery names</param>
        /// <param name="propertiesValues">List of values for corresponding property name</param>
        public static void ShowItems(List<List<string>> propertiesNames, List<string> propertiesValues)
        {
            foreach(List<string> properties in propertiesNames)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine($"{properties[i]}: {propertiesValues[i]}");
                }
            }
        }

        /// <summary>
        /// Saving list of items in XLSX (excel) file
        /// </summary>
        /// <param name="items">List that include values collection for each item</param>
        /// <param name="propertiesNames">Names of values for given item type</param>
        /// <param name="fileName">File name</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void SaveAsXlsxFile(List<List<string>> items, List<string> propertiesNames, string fileName)
        {
            try
            {
                if (fileName == String.Empty || fileName == null)
                {
                    throw new NullReferenceException();
                }

                fileName += ".xlsx";

                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, fileName);

                FileInfo file = new FileInfo(filePath);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet;

                    //If worksheet is not exist then create one
                    if (package.Workbook.Worksheets.Count() > 0)
                    {
                        worksheet = package.Workbook.Worksheets.Add("Sheet-1");
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets["Sheet-1"];
                    }

                    //If header is empty, then fill it up
                    if (worksheet.Cells[1, 1].Value == null)
                    {
                        for (int column = 1; column <= propertiesNames.Count; column++)
                        {
                            worksheet.Cells[1, column].Value = propertiesNames[column - 1];
                        }
                    }

                    //If file alreay contains data, then find first empty row to start filling
                    int firstEmptyRow = 2;

                    while(true)
                    {
                        if(worksheet.Cells[firstEmptyRow, 1].Value == null)
                        {
                            break;
                        }
                        
                        firstEmptyRow++;
                    }

                    for (int row = firstEmptyRow; row < items.Count + firstEmptyRow; row++)
                    {
                        for (int column = 1; column <= propertiesNames.Count; column++)
                        {
                            worksheet.Cells[row, column].Value = items[row - firstEmptyRow][column - 1];
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in method \"SaveAsXlsxFile\": {ex.Message}");
            }
        }

        /// <summary>
        /// Saving comma separated propertiy values of an items collection in CSV file
        /// </summary>
        /// <param name="items">Collection of strings that represent's velues of each item</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task SaveInCsvFile(List<string> items, string fileName)
        {
            try
            {
                if (fileName == "" || fileName == null)
                {
                    throw new NullReferenceException();
                }

                fileName += ".csv";

                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, fileName);

                await File.AppendAllLinesAsync(filePath, items, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in method \"SaveInCsvFile\": {ex.Message}");
            }
        }

        /// <summary>
        /// Saving items in XML or JSON format in text file
        /// </summary>
        /// <param name="items"></param>
        /// <param name="fileName"></param>
        /// <exception cref="NullReferenceException"></exception>
        public static async void SaveInTextFile(List<string> items, string fileName)
        {
            try
            {
                if(String.IsNullOrEmpty(fileName))
                {
                    throw new NullReferenceException();
                }

                fileName += ".txt";

                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, fileName);

                await File.AppendAllLinesAsync(filePath, items, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in method \"SaveInTextFile\": {ex.Message}");
            }
        }

        /// <summary>
        /// Converting class into JSON string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertToJson<T>(T item)
        {
            var jsonItem = "";

            try
            {
                if (item == null)
                {
                    throw new NullReferenceException();
                }

                jsonItem = JsonConvert.SerializeObject(item);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Error in method \"ConvertToJson\": {ex.Message}");
                jsonItem = $"Json convertion error. Item is null.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in method \"ConvertToJson\": {ex.Message}");
                jsonItem = $"Json convertion error in item: {item}";
            }

            return jsonItem;
        }

        /// <summary>
        /// Converting class into XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertToXml<T>(T item)
        {
            var xmlItem = "";

            try
            {
                if (item == null)
                {
                    throw new NullReferenceException();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringWriter stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, item);

                    xmlItem = stringWriter.ToString();
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Error in method \"ConvertToXml\": {ex.Message}");
                xmlItem = $"Xml convertion error. Item is null.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in method \"ConvertToXml\": {ex.Message}");
                xmlItem = $"Xml convertion error in item: {item}";
            }

            return xmlItem;
        }
    }
}
