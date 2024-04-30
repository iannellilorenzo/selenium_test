using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    public static void Main()
    {
        // come link prendo https://stackoverflow.com/questions, da qui entro in ogni domanda e dopo che sono entrato ci prendo il titolo della domanda, il corpo della domanda, gli upvote, e gli stessi campi della risposta flaggata come utile. id per cookies: onetrust-accept-btn-handler
        IWebDriver driver = new ChromeDriver();
        string url = "https://stackoverflow.com/questions";
        driver.Navigate().GoToUrl(url);
        Thread.Sleep(3000);
        IWebElement cookies = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
        cookies.Click();

        IWebElement questions = driver.FindElement(By.Id("questions"));
        List<string> quests = new();
        for(int i = 0; i < quests.Count; i++)
        {
            quests.Add(questions.GetAttribute("div"));
        }


        driver.Close();
    }
}