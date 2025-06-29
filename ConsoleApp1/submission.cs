using System;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;



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
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


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
            Console.WriteLine(xsdUrl.Substring(0, xsdUrl.LastIndexOf("/")));
            Console.WriteLine(xsdUrl);
            schema.Add(xsdUrl.Substring(0, xsdUrl.LastIndexOf("/")), xsdUrl);
            XDocument xml = XDocument.Load(xmlUrl);
            string validationOut = "No Error";
            xml.Validate(schema, (o, e) =>
            {
                validationOut = e.Message;
            });
            return validationOut;

            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlUrl);
            string jsonText = JsonConvert.SerializeXmlNode(xml);
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            return jsonText;

        }
    }

}
