using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Variable;
using BoDi;
using TestAutomation.StepDefinitions;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.IO;
using Utility;
using TechTalk.SpecFlow;
using AventStack.ExtentReports.Gherkin.Model;

namespace TestAutomation.Hooks {

    [Binding]
    class TestHooks : ExtentReport {

        private IObjectContainer _objectContainer;
        public static IWebDriver driver;
        public WebDriverWait wait;
        private ExtentTest test;

        private CommonSteps common;

        private const string AdminAccountUrl = "https://front.serverest.dev/admin/home";
        private const string RegularAccountUrl = "https://front.serverest.dev/home";
        private const string LoginUrl = "https://front.serverest.dev/login";

        public TestHooks(IObjectContainer objectContainer) {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void InitializeReport() {
            Console.WriteLine("Running before test run...");
            ExtentReportInit();
        }

        [BeforeFeature]
        public static void InitializeWebDriver(FeatureContext context) {
            ChromeOptions chromeOptions = new ChromeOptions {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(Variables.URL);

            Console.WriteLine("Running before feature...");
            _feature = _extentReports.CreateTest<Feature>(context.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void RegisterDriver(ScenarioContext context) {
            _objectContainer.RegisterInstanceAs<IWebDriver>(driver);

            _scenario = _feature.CreateNode<Scenario>(context.ScenarioInfo.Title);
            test = _scenario;
        }

        [AfterScenario]
        public void AfterScenario(FeatureContext featureContext, ScenarioContext scenarioContext) {
            try {
                NavigateBasedOnFeature(featureContext.FeatureInfo.Title);

                if (scenarioContext.TestError != null) {
                    string errorMessage = scenarioContext.TestError.Message;
                    string stackTrace = scenarioContext.TestError.StackTrace;

                    test.Fail($"Test failed with error: {errorMessage}")
                        .Fail($"Stack trace: {stackTrace}");

                    var screenshot = MediaEntityBuilder.CreateScreenCaptureFromBase64String(TakeScreenshot(driver, scenarioContext)).Build();
                    test.Fail("Screenshot at failure", screenshot);
                }
                else {
                    // Se o teste passou, registra o sucesso
                    test.Pass("Test Passed");
                }
            }
            catch (WebDriverException e) {
                Console.WriteLine($"WebDriver error in AfterScenario: {e.Message}");
            }
            catch (Exception e) {
                Console.WriteLine($"Error in AfterScenario: {e.Message}");
            }
        }


        private void NavigateBasedOnFeature(string featureTitle) {
            if (driver != null && driver.Url != null) {
                switch (featureTitle) {
                    case "Administrator Account":
                        driver.Navigate().GoToUrl(AdminAccountUrl);
                        Console.WriteLine("Navigated to Administrator Account home page.");
                        break;
                    case "Regular Account":
                        driver.Navigate().GoToUrl(RegularAccountUrl);
                        Console.WriteLine("Navigated to Regular Account home page.");
                        break;
                    case "Login":
                        driver.Navigate().GoToUrl(LoginUrl);
                        Console.WriteLine("Navigated to Login page.");
                        break;
                    default:
                        Console.WriteLine("Feature not recognized. No navigation performed.");
                        break;
                }
            }
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext, IWebDriver driver) {
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var screenshot = MediaEntityBuilder.CreateScreenCaptureFromBase64String(TakeScreenshot(driver, scenarioContext)).Build();

            if (scenarioContext.TestError != null) {
                string errorMessage = scenarioContext.TestError.Message;
                string stackTrace = scenarioContext.TestError.StackTrace;

                switch (stepType) {
                    case "Given":
                        _scenario.CreateNode<Given>(stepName).Fail($"Error: {errorMessage}")
                            .Fail($"Stack trace: {stackTrace}", screenshot);
                        break;
                    case "When":
                        _scenario.CreateNode<When>(stepName).Fail($"Error: {errorMessage}")
                            .Fail($"Stack trace: {stackTrace}", screenshot);
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(stepName).Fail($"Error: {errorMessage}")
                            .Fail($"Stack trace: {stackTrace}", screenshot);
                        break;
                    default:
                        _scenario.CreateNode<And>(stepName).Fail($"Error: {errorMessage}")
                            .Fail($"Stack trace: {stackTrace}", screenshot);
                        break;
                }
            }
            else {
                switch (stepType) {
                    case "Given":
                        _scenario.CreateNode<Given>(stepName).Pass("Success", screenshot);
                        break;
                    case "When":
                        _scenario.CreateNode<When>(stepName).Pass("Success", screenshot);
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(stepName).Pass("Success", screenshot);
                        break;
                    default:
                        _scenario.CreateNode<And>(stepName).Pass("Success", screenshot);
                        break;
                }
            }
        }

        private static void CaptureStepScreenshot(ScenarioContext scenarioContext, IWebDriver driver, string stepType, string stepName) {
            var screenshot = MediaEntityBuilder.CreateScreenCaptureFromBase64String(TakeScreenshot(driver, scenarioContext)).Build();

            switch (stepType) {
                case "Given":
                    _scenario.CreateNode<Given>(stepName).Pass("Success", screenshot);
                    break;
                case "When":
                    _scenario.CreateNode<When>(stepName).Pass("Success", screenshot);
                    break;
                case "Then":
                    _scenario.CreateNode<Then>(stepName).Pass("Success", screenshot);
                    break;
                default:
                    _scenario.CreateNode<And>(stepName).Pass("Success", screenshot);
                    break;
            }
        }

        [AfterTestRun]
        public static void FlushReport() {
            ExtentReportTearDown();
        }

        [AfterFeature]
        public static void TearDown() {
            try {
                Console.WriteLine("Tentando executar Flush do ExtentReports...");
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro no Flush do ExtentReports: {ex.Message}");
            }
            finally {
                QuitDriver();
            }
        }

        private static void QuitDriver() {
            if (driver != null) {
                driver.Quit();
                driver = null;
            }
        }
    }
}