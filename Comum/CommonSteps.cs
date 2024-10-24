using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using BoDi;
using OpenQA.Selenium.Interactions;

namespace TestAutomation.StepDefinitions {

    [Binding]
    public class CommonSteps {

        private IObjectContainer _objectContainer;
        public IWebDriver driver;
        public WebDriverWait wait;

        public CommonSteps(IWebDriver driver) {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        }

        public void BeAtTab(string nameTab) {
            try {
                driver.FindElement(By.XPath($"//a[contains(.,'{nameTab}')]")).Click();
                Console.WriteLine($"Navigated to {nameTab} tab successfully.");
            }
            catch (NoSuchElementException ex) {
                Console.WriteLine($"Tab '{nameTab}' not found: {ex.Message}");
                throw;
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred while navigating to '{nameTab}' tab: {ex.Message}");
                throw;
            }
        }

        public void ScrollElementIntoView(IWebElement element) {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public bool ScrollElementIntoViewStatus(By locator) {
            try {
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                IWebElement element = wait.Until(driver => driver.FindElement(locator));

                if (element.Displayed) {
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(element).Perform();
                    return true;
                }
                return false;
            }
            catch (OpenQA.Selenium.NoSuchElementException) {
                return false;
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException) {
                return false;
            }
        }

        public bool IsUserLoggedIn() {
            try {
                return driver.FindElement(By.XPath("//a[contains(.,'Home')]")).Displayed;
            }
            catch (NoSuchElementException) {
                return false;
            }
        }

        public bool IsElementPresent(By by) {
            try {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException) {
                return false;
            }
        }

        public IList<IWebElement> WaitForElementsToBeVisible(By locator) {
            return wait.Until(driver =>
            {
                var elements = driver.FindElements(locator);
                return elements.All(e => e.Displayed) ? elements : null;
            });
        }

    }
}
