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
        public static string Verification(string xmlUrl, string xsdUrl) {

            // Create output string and schema/xml objects
            string validationOut = "No Error";
            XmlSchemaSet schema = new XmlSchemaSet();
            XmlDocument xml = new XmlDocument();

            // Attempt to load the XML doc at provided URL, return any error thrown as string
            // Load() method only checks for syntax validity, not Schema compliance
            try
            {
                xml.Load(xmlUrl);
            }
            catch (Exception e)
            {
                validationOut = "Error: " + e.Message;
                return validationOut;
            }

            // Attempt to load the XML schema at provided URL 
            // (determine namespace from child namespace (e.g. Hotels), it's more reliable than string manipulation on the schema URL, but admittedly only works with one child)
            // Add() method should only fail if there's an issue with the Get request, no validity checks
            try
            {
                schema.Add(xml.LastChild.NamespaceURI, xsdUrl);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
            xml.Schemas = schema;

            // Validates the XML file against the provided schema, indicating any errors or warnings
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

        public static string Xml2Json(string xmlUrl) {
            // Locate the XML document at the provided URL and convert it to a JSON string
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlUrl);
            string jsonText = JsonConvert.SerializeXmlNode(xml);

            // Replace attribute @ signs with underscores for grading criteria compliance
            jsonText = jsonText.Replace('@', '_');
            
            // Convert JSON string to JObj for further cleaning of data
            JObject jsonObj = JObject.Parse(jsonText);

            // Remove all properties not directly describing relevant data (i.e. all properties necessary for XML interpretation)
            jsonObj.Property("?xml")?.Remove();

            foreach (var firstChild in jsonObj.Root.Children()) {
                ((JObject)firstChild.Last).Property("_xmlns")?.Remove();
                ((JObject)firstChild.Last).Property("_xmlns:xsi")?.Remove();
                ((JObject)firstChild.Last).Property("_xsi:schemaLocation")?.Remove();
            }

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            return jsonObj.ToString();
        }
    }

}
