using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using OpenQA.Selenium.DevTools.V121.Audits;
using OpenQA.Selenium.DevTools.V121.DOM;
using System.Security.Principal;

class Item
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Upvotes { get; set; }
    public string FlaggedAnswer { get; set; }
}

class Program
{
    public static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        string url = "https://stackoverflow.com/questions"; // debug url = "https://stackoverflow.com/questions/11828270/how-do-i-exit-vim"
        driver.Navigate().GoToUrl(url);
        Thread.Sleep(3000);

        // Accept cookies
        IWebElement cookies = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
        cookies.Click();

        // Get question elements
        ReadOnlyCollection<IWebElement> questionElements = driver.FindElements(By.CssSelector(".s-post-summary"));

        // Create a list to store the question objects
        List<Item> items = new();
        int index = 0;

        // Iterate through each question
        foreach (IWebElement questionElement in questionElements)
        {
            if (index == 3)
            {
                break;
            }

            // Navigate inside the question
            IWebElement a = questionElement.FindElement(By.TagName("a"));
            driver.Navigate().GoToUrl(a.GetAttribute("href"));
            Thread.Sleep(5000);

            // Get question title
            string questionTitle = driver.FindElement(By.Id("question-header")).Text;

            // Get question body
            string questionBody = driver.FindElement(By.ClassName("js-post-body")).Text;

            // Get question upvotes
            string questionUpvotes = driver.FindElement(By.ClassName("js-vote-count")).Text;

            // Get flagged answers
            string flaggedAnswer = "";
            try
            {
                flaggedAnswer = driver.FindElement(By.ClassName("accepted-answer")).Text;
            }
            catch (NoSuchElementException)
            {
                flaggedAnswer = "Not found";
            }

            // Create a item object
            Item item = new Item
            {
                Title = questionTitle,
                Body = questionBody,
                Upvotes = questionUpvotes,
                FlaggedAnswer = flaggedAnswer
            };

            items.Add(item);

            Thread.Sleep(3000);
            driver.Navigate().Back();
            index++;
        }

        // Serialize the list of questions to JSON using Newtonsoft.Json
        string json = JsonConvert.SerializeObject(items, Formatting.Indented);
        File.WriteAllText("data.json", json);

        // Print the JSON string
        Console.WriteLine(json);

        Thread.Sleep(5000);
        driver.Close();
    }
}
