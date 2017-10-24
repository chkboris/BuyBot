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

            List<Clothing> catalog = new List<Clothing>();
            foreach(XElement xeArticle in xeDiv.Elements("article"))
            {
                Clothing article = grab(xeArticle);
                if(article != null)
                    catalog.Add(grab(xeArticle));
            }
            saveFile(catalog);
        }

        static Clothing grab(XElement article)
        {
            XElement inner = article.Element("div");
            XElement a = inner.Element("a");
            XElement sold = a.Element("div");
            if (sold != null)
                return null;
            //Get the link to the Jacket
            String href = a.Attribute("href").Value;
            //Get the image of the jacket
            XElement pic = a.Element("img");
            String image = pic.Attribute("src").Value;
            Clothing clothingItem = new Clothing(href, image);
            return clothingItem;
        }

        static void saveFile(List<Clothing> catalog)
        {            
            String path = "catalog.html";           
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head></head>");
            sb.AppendLine("<body>");
            sb.AppendLine();
            String lastStyle = "";
            String lastCategory = "";
            foreach (Clothing item in catalog)
            {
                String[] urlInParts = item.getUrl().Split('/');
                String style = urlInParts[urlInParts.Length - 2];
                String category = urlInParts[urlInParts.Length - 3];
                if(!category.Equals(lastCategory))
                {
                    sb.Append("<h3>" + category + "</h3>" + "\n<hr>");
                }
                lastCategory = category;
                if (!style.Equals(lastStyle))
                {
                    sb.Append("<br />");
                }
                lastStyle = style;
                sb.AppendLine("<a href =\"http://www.supremenewyork.com" + item.getUrl() +"\">");
                sb.AppendLine("<img src=\"images/" + item.getShortImg() +"\"" + " style=\"width:100px;height:100px;\" border=\"5\">");
                sb.AppendLine("</a>");

                sb.AppendLine();
            }
            sb.AppendLine("</body>\n</html>");
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.Write(sb.ToString());
            }
            using (WebClient client = new WebClient())
            {
                int counter = 0;                
                foreach (Clothing item in catalog)
                {
                    counter++;
                    client.DownloadFile("http:" + item.getImg(), "images\\" + item.getShortImg());

                }               
            }
        }
    }
}

/*
 *         <article>
          <div class="inner-article">
            <a style="height:81px;" href="/shop/jackets/xobhixzgc/uoaenhqpi">
              <img width="81" height="81" src="//d17ol771963kd3.cloudfront.net/141098/vi/E4GBOddiaDw.jpg" alt="E4gboddiadw" />
              <div class="sold_out_tag">sold out</div>
            </a>
          </div>
        </article>
*/