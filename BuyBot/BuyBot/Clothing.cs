using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyBot
{
    class Clothing
    {
        double price;
        String urlExtention;
        String imgPath;
        List<String> keywords = new List<string>();

        public Clothing(String extention, String imagePath)
        {
            urlExtention = extention;
            imgPath = imagePath;
        }

        public void setPrice(double newPrice)
        {
            price = newPrice;
        }
        public void addKeyword(String newWord)
        {
            keywords.Add(newWord);
        }
        public String getUrl()
        {
            return urlExtention;
        }
        public String getImg()
        {
            return imgPath;
        }
        public String getShortImg()
        {
            
            return Path.GetFileName(imgPath);
        }
    }
}
