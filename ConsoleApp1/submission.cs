using System;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static string xmlURL = "https://polygeist111.github.io/ASU_CSE445_Assignment_4/Hotels.xml";
        public static string xmlErrorURL = "https://polygeist111.github.io/ASU_CSE445_Assignment_4/HotelsError.xml";
        public static string xsdURL = "https://polygeist111.github.io/ASU_CSE445_Assignment_4/Hotels.xsd";

        public static void Main(string[] args)
        {
            Console.WriteLine("Validating Correct File");
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            Console.WriteLine("Validating Error File");
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            Console.WriteLine("Parsing Correct File to JSON");
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            /*using (var client = new HttpClient()) {
                HttpResponseMessage response = await client.GetAsync(xmlUrl);
                if (response.IsSuccessStatusCode) {
                    string responseBody = await response.Content.ReadAsStringAsync();
                } else {
                    // Handle the error
                    return response.ReasonPhrase.ToString();
                }
            }*/
            XmlSchemaSet schema = new XmlSchemaSet();
            // assumes namespace is folder xsd is in (not ideal, but necessary due to assignment constraints)
            //Console.WriteLine(xsdUrl.Substring(0, xsdUrl.LastIndexOf("/")));
            //Console.WriteLine(xsdUrl);
            string validationOut = "No Error";
            schema.Add(xsdUrl.Substring(0, xsdUrl.LastIndexOf("/")), xsdUrl);
            XmlDocument xml = new XmlDocument();
            try {
                xml.Load(xmlUrl);
            } catch (Exception e) {
                validationOut = "Error: " + e.Message;
                return validationOut;
            }
            xml.Schemas = schema;
            /*xml.Validate(schema, (o, e) =>
            {
                validationOut = e.Message;
            });*/
            xml.Validate((o, e) => {
                if (e.Severity == XmlSeverityType.Error) {
                    validationOut = "Error: ";
                }
                else if (e.Severity == XmlSeverityType.Warning) {
                    validationOut = "Warning: ";
                }
                validationOut += e.Message;
            });
            return validationOut;

            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlUrl);
            string jsonText = JsonConvert.SerializeXmlNode(xml);
            jsonText = jsonText.Replace('@', '_'); // replaces attribute @ signs with underscores for grading criteria compliance

            JObject jsonObj = JObject.Parse(jsonText);

            jsonObj.Property("?xml")?.Remove();

            foreach (var firstChild in jsonObj.Root.Children()) {
                ((JObject)firstChild.Last).Property("_xmlns")?.Remove();
                ((JObject)firstChild.Last).Property("_xmlns:xsi")?.Remove();
                ((JObject)firstChild.Last).Property("_xsi:schemaLocation")?.Remove();
            }

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            //Console.WriteLine(jsonObj.Root.ToString());

            return jsonObj.ToString();
        }
    }

}
