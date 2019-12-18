using BBCollection;
using BBGatherer.Webcrawler.BilkaToGo;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace BBGatherer.Webcrawler
{
    class BilkaToGoCrawl
    {
        public LinkCreation Links = new LinkCreation();
        public BilkaProducts Products = new BilkaProducts();
    }
}
