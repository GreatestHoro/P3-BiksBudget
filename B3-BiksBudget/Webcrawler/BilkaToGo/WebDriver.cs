using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace BBGatherer.Webcrawler.BilkaToGo
{
    class WebDriver
    {
        int[] _ports = new int[4] { 63255, 63256, 63260, 64158 };
        string _path = @"C:\Users\bbalt\Desktop\Crawl\P3-BiksBudget";

        IWebDriver _driver;
        FirefoxDriverService _service;

        public WebDriver(bool headless)
        {
            if (headless)
            {
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("--headless");
                _service = FirefoxDriverService.CreateDefaultService(_path);
                _driver = new FirefoxDriver(_service, options);
            }
            else
            {
                _service = FirefoxDriverService.CreateDefaultService(_path);
                _driver = new FirefoxDriver(_service);
            }
        }

        public IWebDriver Driver()
        {
            return _driver;
        }

        /*private int GeneratePort()
        {
            int availablePort = 0;
            foreach(int port in _ports)
            {
                if (!CheckPort(port))
                {
                    availablePort = port;
                }
            }
            return availablePort;
            
        }

        private static bool CheckPort(int port)
        {
            bool portInUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    return portInUse = true;
                }
            }
            return portInUse;
        }*/
    }
}
