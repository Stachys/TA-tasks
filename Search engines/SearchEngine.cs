﻿using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;

namespace lab_ta_homework_5.Search_engines
{
    class SearchEngine
    {
        protected IWebDriver driver;

        protected string Url { get; set; }

        protected string SearchFieldXPath { get; set; }

        protected string ResultsXPath { get; set; }

        protected string NextXPath { get; set; }

        protected string PageNumXPath { get; set; }

        private string ToSearch { get; set; }

        private string ToFind { get; set; }

        private string PathToSave { get; set; }

        protected SearchEngine()
        {
            driver = Driver.driver;
        }

        public void Search(string toSearch) 
        {
            ToSearch = toSearch;
            IWebElement searchField = driver.FindElement(By.XPath(SearchFieldXPath));
            searchField.SendKeys(ToSearch);
            searchField.SendKeys(Keys.Enter);
        }

        public int VerifyResults(string toFind, bool screenshotAllPages)
        {
            ToFind = toFind;
            PathToSave = String.Format(Constants.pathToImages, Environment.UserName, this.GetType().Name, DateTime.Now.ToString(Constants.dateTime), ToFind, ToSearch);
            Directory.CreateDirectory(PathToSave);
            IList<IWebElement> next;
            do
            {
                string pageNum;
                IList<IWebElement> results = driver.FindElements(By.XPath(ResultsXPath));
                if (results.Count == 0)
                {
                    pageNum = "no result";
                }
                else
                {
                    pageNum = driver.FindElement(By.XPath(PageNumXPath)).Text;
                }

                foreach (IWebElement result in results)
                {
                    if (result.Text.Contains(ToFind))
                    {
                        Console.WriteLine(String.Format(Constants.foundToConsole, ToFind, ToSearch, pageNum));
                        Driver.GetScreenshot().Save(String.Format(Constants.imageNameFound, PathToSave, pageNum));
                        return Int32.Parse(pageNum);
                    }
                }

                if (screenshotAllPages)
                {
                    Driver.GetScreenshot().Save(String.Format(Constants.imageNameNotFound, PathToSave, pageNum));
                }

                next = driver.FindElements(By.XPath(NextXPath));
                if (next.Count != 0)
                {
                    next[0].Click();
                }

            } while (next.Count != 0);

            return 0;
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
