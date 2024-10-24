using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Variable;
using TestAutomation.StepDefinitions;
using Locators;

namespace StepDefinitions {

    [Binding]
    public class RegularAccountPO {

        private IWebDriver driver;
        private WebDriverWait wait;

        AdministratorAccountPage administratorAccount;
        CommonSteps common;

        public RegularAccountPO(IWebDriver driver) {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            this.administratorAccount = new AdministratorAccountPage(driver);
            this.common = new CommonSteps(driver);
        }

        [Given(@"I am logged in as regular")]
        public void IAmLoggedInAsRegular() {
            if (driver.Url.Contains("Home") || common.IsUserLoggedIn()) {
                Console.WriteLine("User already logged in, skipping login.");
                return;
            }

            string regularName = Variables.REGULAR_NAME;
            string regularUser = Variables.REGULAR_USER;
            string adminPass = Variables.REGULAR_PASS;

            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["inputEmail"])).SendKeys(regularUser);
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["inputPassword"])).SendKeys(adminPass);
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["butEnter"])).Click();

            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            try {
                common.WaitForElementsToBeVisible(By.XPath(RegularAccountLocator.LoginReg["messageInvalid"]));
                RegisterNewAccountReg(regularName, regularUser, adminPass);
            }
            catch (WebDriverTimeoutException) {}
        }

        private void RegisterNewAccountReg(string name, string email, string password) {
            string regularName = Variables.REGULAR_NAME;
            string regularUser = Variables.REGULAR_USER;
            string adminPass = Variables.REGULAR_PASS;

            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["btSign_Up"])).Click();
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["userName"])).SendKeys(regularName);
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["userEmail"])).SendKeys(regularUser);
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["userPassword"])).SendKeys(adminPass);
            SendRegistration();
        }

        public void SendRegistration() {
            driver.FindElement(By.XPath(RegularAccountLocator.LoginReg["btRegister"])).Click();
            common.WaitForElementsToBeVisible(By.XPath(RegularAccountLocator.LoginReg["ms_Cad_Suc"]));
            ThePageShouldLoadCorrectly();
        }

        [Then("The page should load correctly")]
        public void ThePageShouldLoadCorrectly() {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hCadUse"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hLisUse"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hCadProd"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hLisProd"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hRel"]));
        }

        [Given(@"On the (.+) tab")]
        public void OnTheTab(string nameTab) {
            driver.FindElement(By.XPath($"//a[contains(text(),'{nameTab}')]")).Click();
        }

        [StepDefinition(@"Click in (.+)")]
        public void Click(string button) {
            driver.FindElement(By.XPath($"//button[contains(text(),'{button}')]")).Click();
        }


        [When("Search for a product that does not exist")]
        public void SearchForAProductThatDoesNotExist() {
            driver.FindElement(By.XPath(RegularAccountLocator.HomeReg["inputSear"])).SendKeys("not exist");
        }

        [StepDefinition("Search")]
        public void Search() {
            driver.FindElement(By.XPath(RegularAccountLocator.HomeReg["btSear"])).Click();
        }

        [Then("Show no results")]
        public void ShowNoResults() {
            common.WaitForElementsToBeVisible(By.XPath(RegularAccountLocator.HomeReg["msNoProdu"]));
        }

        [When("Search for a product")]
        public void SearchForAProduct() {
            try {
                driver.FindElement(By.XPath(RegularAccountLocator.HomeReg["inputSear"])).SendKeys("No picture");
                Search();

                bool status = common.ScrollElementIntoViewStatus(By.XPath(RegularAccountLocator.HomeReg["msNoProdu"]));

                if (status) {
                    Click("Logout");
                    common.WaitForElementsToBeVisible(By.XPath(RegularAccountLocator.LoginReg["login"]));

                    administratorAccount.IAmLoggedInAsAdministrator();
                    OnTheTab("Cadastrar Produto");
                    administratorAccount.FillInProduct("no picture");
                    administratorAccount.ClickButtonRegister();
                    administratorAccount.ProductMustBeOnTheList();

                    Click("Logout");
                    IAmLoggedInAsRegular();
                    OnTheTab("Home");

                    driver.FindElement(By.XPath(RegularAccountLocator.HomeReg["inputSear"])).SendKeys("no picture");
                    Search();
                }
            }
            catch (WebDriverTimeoutException) {}
        }

        [StepDefinition("Detail the product")]
        public void DetailTheProduct() {
            driver.FindElement(By.XPath(RegularAccountLocator.HomeReg["detail"])).Click();
        }

        [Then("Product details must be visible")]
        public void ProductDetailsMustBeVisible() {
            common.WaitForElementsToBeVisible(By.XPath(RegularAccountLocator.HomeReg["msDetail"]));
        }

        [Then(@"The product must be in the shopping cart")]
        public void TheProductMustBeInTheShoppingCart() {
            Console.WriteLine("Unfortunately, this page is not done");
        }

    }
}
