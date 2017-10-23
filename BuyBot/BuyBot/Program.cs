// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

//using SHDocVw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Xml.Linq;

/*
Stopped on Oct 22, currently trying to place images and links from supreme website into a directory with an html file
*/


namespace BuyBot
{
    class Program
    {

        static void Main()
        {
            string rawHtml = string.Empty;
            string url = "http://www.supremenewyork.com/shop/all";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                rawHtml = reader.ReadToEnd();
            }
            //Console.WriteLine(rawHtml);
            //Console.ReadLine();


            int start = rawHtml.IndexOf("<div id=\"wrap\">");
            int length = rawHtml.IndexOf("<footer") - start;
            String html = rawHtml.Substring(start,length);
            XElement xeDocument = XElement.Parse(html.Replace("&nbsp", " "));
            XElement xeDiv = xeDocument.Element("div");

            Dictionary<String, String> clothes = new Dictionary<String, String>();
            foreach(XElement xeArticle in xeDiv.Elements("article"))
            {
                String[] item = grabImage(xeArticle);
                if(item != null)
                {
                    clothes.Add(item[1], item[0]);
                }               
            }

            //Create HTML
            foreach(String image in clothes.Keys)
            {

            }
            Console.WriteLine(clothes.Count + " items available");
        }

        static String[] grabImage(XElement article)
        {
            string[] info = new string[2];
            XElement inner = article.Element("div");
            XElement a = inner.Element("a");
            XElement sold = a.Element("div");
            if (sold != null)
                return null;
            //Get the link to the Jacket
            String href = a.Attribute("href").Value;
            info[0] = href;
            //Get the image of the jacket
            XElement pic = a.Element("img");
            String image = pic.Attribute("src").Value;
            info[1] = image;
            return info;
        }

        static void saveFile()
        {

        }
    }
}