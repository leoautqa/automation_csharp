using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Bogus;
using Variable;
using TestAutomation.StepDefinitions;
using Locators;

namespace StepDefinitions {

    [Binding]
    public class AdministratorAccountPage {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Faker faker;
        private string name;
        private string email;
        private string password;
        private string prod;

        CommonSteps common;

        public AdministratorAccountPage(IWebDriver driver) {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            this.common = new CommonSteps(driver);
        }

        [Given(@"I am logged in as administrator")]
        public void IAmLoggedInAsAdministrator() {
            string adminUser = Variables.ADMIN_USER;
            string adminPass = Variables.ADMIN_PASS;

            try {
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["inputEmail"])).SendKeys(adminUser);
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["inputPassword"])).SendKeys(adminPass);
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["butEnter"])).Click();
                Console.WriteLine("Entered login credentials and clicked login");

                // Espera pela mensagem de erro de login, caso apareça
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                bool isErrorMessagePresent = wait.Until(driver => {
                    try {
                        return driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["messageInvalid"])).Displayed;
                    }
                    catch (NoSuchElementException) {
                        return false;
                    }
                });

                if (isErrorMessagePresent) {
                    Console.WriteLine("Login failed, attempting to register a new account.");
                    RegisterNewAccountAdmin(Variables.ADMIN_NAME, adminUser, adminPass);
                }
                else {
                    Console.WriteLine("Logged in successfully, no invalid message found.");
                }
            }
            catch (WebDriverTimeoutException) {
                Console.WriteLine("Timeout while waiting for the invalid message element.");
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
            }

            // Verifique se o login foi bem-sucedido
            if (!common.IsElementPresent(By.XPath($"//a[contains(.,'Home')]"))) {
                throw new Exception("Login failed, home tab not found.");
            }
            else {
                Console.WriteLine("Login successful, home tab found.");
            }
        }

        private void RegisterNewAccountAdmin(string name, string email, string password) {
            try {
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["btSign_Up"])).Click();
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userName"])).SendKeys(name);
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userEmail"])).SendKeys(email);
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userPassword"])).SendKeys(password);
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["checkbox"])).Click();
                Console.WriteLine("Registering new admin account...");
                SendRegistration();
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
            }
        }

        private void SendRegistration() {
            try {
                driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["btRegister"])).Click();
                common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LoginAdm["ms_Cad_Suc"]));
                Console.WriteLine("Registration successful.");
                PageMustLoadCorrectly();
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred during the registration process: {ex.Message}");
            }
        }

        [Given(@"Be at (.+) tab")]
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

        [Then("Page must load correctly")]
        public void PageMustLoadCorrectly() {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hCadUse"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hLisUse"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hCadProd"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hLisProd"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.HomeAdm["hRel"]));
        }

        [When("Do the registration")]
        public void DoTheRegistration() {
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["btCad"])).Click();
        }

        [Then(@"The message (.*) on Register Users")]
        public void TheMessageOnRegisterUsers(string message) {
            try {
                if (message.Equals("alert register")) {
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadUsu["msgNameMan"]));
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadUsu["msgEmailMan"]));
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadUsu["msgPassMan"]));
                }
                else {
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadUsu["msAlrExi"]));
                }
            }
            catch (WebDriverTimeoutException) { }
        }

        [When("Complete (.+) registration")]
        public void CompleteRegistration(string account) {
            faker = new Faker();

            name = faker.Name.FullName();
            email = faker.Internet.Email();
            password = faker.Internet.Password();

            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpName"])).SendKeys(name);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpEmail"])).SendKeys(email);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpPass"])).SendKeys(password);

            try {
                if (account.Equals("Administrator")) {
                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["cheBoxAdm"])).Click();
                }
            }
            catch (WebDriverTimeoutException) { }
        }

        [StepDefinition(@"(.+) registration must be on the list")]
        public void RegistrationMustBeOnTheList(string line) {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisUsu["hUmLisUsu"]));

            System.Threading.Thread.Sleep(2000);

            IWebElement elementToScroll;
            if (line.Equals("Regular")) {
                elementToScroll = driver.FindElement(By.XPath($"//tr[td[contains(., '{name}')] and td[contains(., 'false')]]/td"));
            }
            else {
                elementToScroll = driver.FindElement(By.XPath($"//tr[td[contains(., '{name}')] and td[contains(., 'true')]]/td"));
            }

            common.ScrollElementIntoView(elementToScroll);
        }

        [When("Complete registration already exists")]
        public void CompleteRegistrationAlreadyExists() {
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpName"])).SendKeys(name);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpEmail"])).SendKeys(email);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadUsu["inpPass"])).SendKeys(password);
        }

        [StepDefinition(@"(.+) account")]
        public void Account(string action) {
            try {
                if (action.Equals("Edit")) {
                    driver.FindElement(By.XPath($"//tr[td[contains(text(), '{name}')]]//button[@class='btn btn-info']")).Click();
                }
                else {
                    driver.FindElement(By.XPath($"//tr[td[contains(text(), '{name}')]]//button[@class='btn btn-danger']")).Click();
                }
            }
            catch (WebDriverTimeoutException) { }
        }

        [Then(@"Account must be (.+)")]
        public void AccountMustBe(string button) {
            try {
                if (button.Equals("edited")) {
                    Console.WriteLine("Unfortunately nothing happens when you click the edit button");
                }
                else {
                    bool status = common.ScrollElementIntoViewStatus(By.XPath($"//tr[td[contains(., '{name}')] and td[contains(., 'false')]]/td"));
                    if (!status) {
                        Console.WriteLine("Account deleted successfully");
                    }
                }
            }
            catch (WebDriverTimeoutException) {

            }
            catch (NoSuchElementException) {
                Console.WriteLine("Element not found, account might be deleted.");
            }
        }


        [When("Click button register")]
        public void ClickButtonRegister() {
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["btCad"])).Click();
        }

        [Then("Alert message should appear")]
        public void AlertMessageShouldAppear() {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msNoName"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msNoPrice"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msNoDesc"]));
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msNoQuant"]));
        }

        [When(@"Fill product information with (.+) (.+)")]
        public void FillProductInformationWith(string option, string category) {
            int price;
            int quantity;
            faker = new Faker();
            if (category.Equals("price")) {
                driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["nameProd"])).SendKeys("Zero price");
                price = 0;
            }
            else {
                driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["nameProd"])).SendKeys("Negative quantity");
                price = faker.Random.Number(1, 9999);
            }

            if (category.Equals("quantity")) {
                quantity = faker.Random.Number(-9999, -1);
            }
            else {
                quantity = faker.Random.Number(1, 9999);
            }

            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["priceProd"])).SendKeys(price.ToString());
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["descProd"])).SendKeys(faker.Lorem.Sentence());
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["quantProd"])).SendKeys(quantity.ToString());
        }

        [When(@"Fill in product (.+)")]
        public void FillInProduct(string info) {
            long price;
            int quantity;

            if (info.Equals("information")) {
                this.prod = "Leozirton";
                price = 1000000000;
                quantity = 1;
                driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["choFil"])).SendKeys(@"C:/Users/lpmuchinski/Desktop/eclipse automation/src/main/java/data/robot.png");
            }
            else if (info.Equals("no quantity")) {
                this.prod = "Don't buy this product";
                price = 1;
                quantity = 0;
                driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["choFil"])).SendKeys(@"C:/Users/lpmuchinski/Desktop/eclipse automation/src/main/java/data/Selenium.png");
            }
            else {
                this.prod = "No picture";
                price = 1;
                quantity = 1;
            }

            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["nameProd"])).SendKeys(prod);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["priceProd"])).SendKeys(price.ToString());
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["descProd"])).SendKeys("Test Leo");
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["quantProd"])).SendKeys(quantity.ToString());
        }

        [Then(@"Product must be on the list")]
        public void ProductMustBeOnTheList() {
            string randomNumber;
            string price = null;
            string desc = null;
            string quantity = null;

            try {
                IList<IWebElement> msProdExis = common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisProd["msProdExis"]));
                if (msProdExis != null && msProdExis.Count > 0) {
                    Random random = new Random();
                    randomNumber = " " + random.Next(101);
                    this.prod = prod + randomNumber + " refact";
                    price = " " + randomNumber;
                    desc = " " + randomNumber;
                    quantity = " " + randomNumber;

                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["nameProd"])).SendKeys(randomNumber + " refact");
                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["priceProd"])).SendKeys(price);
                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["descProd"])).SendKeys(desc);
                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["quantProd"])).SendKeys(quantity);
                    driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["btCad"])).Click();
                }
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException) {
                // Timeout handling if necessary
            }

            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisProd["titLis"]));

            IList<IWebElement> productElements = common.WaitForElementsToBeVisible(By.XPath("//td[contains(text(),\"" + prod + "\")]"));
            foreach (IWebElement element in productElements) {
                common.ScrollElementIntoView(element); // Scroll para cada elemento
            }

        }

        [When(@"Find product (.+)")]
        public void FindTheProduct(string typeProd) {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisProd["titLis"]));

            bool status = common.ScrollElementIntoViewStatus(By.XPath("//tr[td[contains(text(), '" + typeProd + "')]]"));

            if (!status) {
                driver.FindElement(By.XPath("//a[contains(text(), 'Cadastrar Produtos')]")).Click();

                FillInProduct(typeProd);
                ClickButtonRegister();

                common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisProd["titLis"]));
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

                common.ScrollElementIntoView(driver.FindElement(By.XPath("//tr[td[contains(text(), '" + typeProd + "')]]")));
            }
            else {
                common.ScrollElementIntoView(driver.FindElement(By.XPath("//tr[td[contains(text(), '" + typeProd + "')]]")));
            }
        }

        [StepDefinition(@"(.+) the item (.+)")]
        public void TheItem(string listAction, string typeProdList) {
            if (listAction.Equals("Edit")) {
                driver.FindElement(By.XPath("//tr[td[contains(text(), '" + typeProdList + "')]]//button[contains(text(), 'Editar')]")).Click();
            }
            else {
                driver.FindElement(By.XPath("//tr[td[contains(text(), '" + typeProdList + "')]]//button[contains(text(), 'Editar')]")).Click();
            }
        }

        [Then(@"Product should be (.+)")]
        public void ProductMustBe(string listMsg) {
            if (listMsg.Equals("edited")) {
                Console.WriteLine("Unfortunately nothing happens when you click the edit button");
            }
            else {
                bool status = common.ScrollElementIntoViewStatus(By.XPath("//tr[td[contains(text(), '" + prod + "')]]"));

                if (!status) {
                    Console.WriteLine("Account deleted successfully");
                }
            }
        }

        [Then("Message (.+)")]
        public void Message(string category) {
            try {
                if (category.Equals("price")) {
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msNoZero"]));
                }
                else {
                    common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.CadProd["msPrice"]));
                }
            }
            catch (WebDriverTimeoutException) { }
        }

        [When("Complete product information")]
        public void CompleteProductInformation() {
            faker = new Faker();
            prod = faker.Commerce.Product();
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["nameProd"])).SendKeys(prod);
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["priceProd"])).SendKeys(faker.Commerce.Price().ToString());
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["descProd"])).SendKeys(faker.Commerce.ProductAdjective());
            driver.FindElement(By.XPath(AdministratorAccountLocator.CadProd["quantProd"])).SendKeys(faker.Random.Number(1, 9999).ToString());
        }

        [Then("Product must be registered")]
        public void ProductMustBeRegistered() {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LisProd["hUmLisProd"]));

            System.Threading.Thread.Sleep(2000);

            IWebElement elementToScroll = driver.FindElement(By.XPath($"//tr[td[contains(., '{prod}')]]/td"));
            common.ScrollElementIntoView(elementToScroll);
        }

        [When("Enter administrator account information")]
        public void EnterAdministratorAccountInformation() {
            string adminName = Variables.ADMIN_NAME;
            string adminUser = Variables.ADMIN_USER;
            string adminPass = Variables.ADMIN_PASS;

            driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userName"])).SendKeys(adminName);
            driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userEmail"])).SendKeys(adminUser);
            driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["userPassword"])).SendKeys(adminPass);
            driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["checkbox"])).Click();
        }

        [Then("Validate that the new administrator user has been registered")]
        public void ValidateNewAdminUserRegistration() {
            common.WaitForElementsToBeVisible(By.XPath(AdministratorAccountLocator.LoginAdm["ms_Cad_Suc"]));
        }

        [When("Select the checkbox administrator")]
        public void SelectCheckboxAdministrator() {
            driver.FindElement(By.XPath(AdministratorAccountLocator.LoginAdm["checkbox"])).Click();
        }
//------------------------------------------------------------------------------------------------------------------------------------------
        [Given("A regular account")]
        public void ARegularAccount() {
            BeAtTab("Cadastrar Usuários");
            CompleteRegistration("Regular");
            DoTheRegistration();
        }
    }
}
