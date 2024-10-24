using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Bogus;
using Locators;
using System.Security.AccessControl;
using TestAutomation.StepDefinitions;

namespace automation_csharp.StepDefinitions {

    [Binding]
    public class loginPO {

        private IWebDriver driver;
        private WebDriverWait wait;
        private Faker faker = new Faker();
        private string name = new Faker().Name.FullName();
        private string email = new Faker().Internet.Email();
        private string password = new Faker().Internet.Password();
        CommonSteps common;

        public loginPO(IWebDriver driver) {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            this.common = new CommonSteps(driver);
        }


        [Given(@"Website loaded")]
        public void WebsiteLoaded() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["h1Login"]));
            common.WaitForElementsToBeVisible((By.XPath(LoginLocator.login["inputEmail"])));
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["inputPassword"]));
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["butEnter"]));
        }

        [When(@"Submit login")]
        public void Submitlogin() {
            driver.FindElement(By.XPath(LoginLocator.login["butEnter"])).Click();
        }

        [Then(@"Mandatory email and password messages")]
        public void MandatoryEmailAndPasswordMessages() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["mandatoryEmail"]));
            common.WaitForElementsToBeVisible((By.XPath(LoginLocator.login["mandatoryPassword"])));
        }

        [When(@"Report invalid data")]
        public void ReportInvalidData() {
            driver.FindElement(By.XPath(LoginLocator.login["inputEmail"])).SendKeys("invalid@test.com");
            driver.FindElement(By.XPath(LoginLocator.login["inputPassword"])).SendKeys("invalid");
        }

        [Then(@"Invalid email and password message")]
        public void InvalidEmailAndPasswordMessage() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["messageInvalid"]));
        }

        [StepDefinition(@"Registering a new profile")]
        public void RegisteringANewProfile() {
            driver.FindElement(By.XPath(LoginLocator.login["register"])).Click();
        }

        [StepDefinition(@"Send registration")]
        public void SendRegistration() {
            driver.FindElement(By.XPath(LoginLocator.login["btRegistration"])).Click();
        }

        [Then(@"Mandatory message")]
        public void MandatoryMessage() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["mandatoryName"]));
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["mandatoryEmail"]));
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["mandatoryPassword"]));
        }

        [StepDefinition(@"Fill in valid data")]
        public void FillInValidData() {
            driver.FindElement(By.XPath(LoginLocator.login["userName"])).SendKeys(name);
            driver.FindElement(By.XPath(LoginLocator.login["userEmail"])).SendKeys(email);
            driver.FindElement(By.XPath(LoginLocator.login["userPassword"])).SendKeys(password);
        }

        [Then(@"Successful registration message")]
        public void SuccessfulRegistrationMessage() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["successfulMessage"]));
        }

        [StepDefinition(@"Fill in data that already exists")]
        public void FillInDataThatAlreadyExists() {
            FillInValidData();
            SendRegistration();

            var elements = common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["successfulMessage"]));
            bool status = elements != null && elements.Count > 0;

            if (status) {
                common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["logout"]));
                driver.FindElement(By.XPath(LoginLocator.login["logout"])).Click();
                WebsiteLoaded();
                RegisteringANewProfile();
                FillInValidData();
            }            
        }

        [Then(@"The message this account already exists")]
        public void MessageThisAccountAlreadyExists() {
            common.WaitForElementsToBeVisible(By.XPath(LoginLocator.login["MessageAlreadyExists"]));
        }

        [StepDefinition(@"Filling an administrator profile")]
        public void FillingAnAdministratorProfile() {
            driver.FindElement(By.XPath(LoginLocator.login["userName"])).SendKeys(name);
            driver.FindElement(By.XPath(LoginLocator.login["userEmail"])).SendKeys(email);
            driver.FindElement(By.XPath(LoginLocator.login["userPassword"])).SendKeys(password);
            driver.FindElement(By.XPath(LoginLocator.login["checkbox"])).Click();
        }
    }
}
