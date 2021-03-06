﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace lab_ta_homework_5.Shopping_websites
{
    abstract class Base
    {
        protected IWebDriver driver;
        protected string Url { get; set; }

        protected string PricesXPath { get; set; }

        protected Base()
        {
            driver = Driver.driver;
            PageFactory.InitElements(driver, this);
        }

        public virtual IEnumerable<int> GetPrices()
        {
            return driver.FindElements(By.XPath(PricesXPath)).Select(p => Int32.Parse(Regex.Replace(p.Text, "[^0-9]", "")));
        }

        public void GoToPage()
        {
            if (Url.Length == 0)
            {
                throw new InvalidOperationException(Constants.noUrlMessage);
            }

            driver.Navigate().GoToUrl(Url);
        }
    }
}
