﻿using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace lab_ta_homework_5.Shopping_websites
{
    class Rozetka : Base
    {
        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Каталог товаров']")]
        private IWebElement cataloge;

        [FindsBy(How = How.XPath, Using = "//ul[@class='menu-categories']/li/a[contains(text(),'Ноутбуки')]")]
        private IWebElement computersMenu;

        [FindsBy(How = How.XPath, Using = "//ul[@class='menu-categories']//a[text()='Ноутбуки']")]
        private IWebElement laptops;

        [FindsBy(How = How.XPath, Using = "//fieldset//input[@formcontrolname='min']")]
        private IWebElement minField;

        private By filterOption = By.XPath("//li[@class='catalog-selection__item']/a");

        private int MinPrice { get; set; }

        public Rozetka() : base()
        {
            Url = Constants.rozetkaUrl;
            PricesXPath = Constants.rozetkaPricesXPath;
        }

        public void CatalogeClick()
        {
            cataloge.Click();
        }

        public void MoveToComputersMenu()
        {
            Actions action = new Actions(driver);
            action.MoveToElement(computersMenu);
            action.Perform();
        }

        public void LaptopsClick()
        {
            laptops.Click();
        }

        public void SetMinPrice(int minPrice)
        {
            MinPrice = minPrice;
            minField.Clear();
            minField.SendKeys(MinPrice.ToString());
        }

        public void SubmitFilter()
        {
            minField.SendKeys(Keys.Enter);
        }

        public override IEnumerable<int> GetPrices()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Constants.explicitWaitSec));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(filterOption, MinPrice.ToString()));

            return base.GetPrices();
        }
    }
}
