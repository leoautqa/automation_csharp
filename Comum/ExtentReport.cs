using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using ImageMagick;

namespace Utility {
    public class ExtentReport {
        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _testResultPath = _baseDir.Replace("bin\\Debug\\net6.0", "TestResults");

        // Informações constantes para o relatório
        private const string _applicationName = "front.serverest";
        private const string _browserName = "Chrome";
        private const string _osName = "Windows";
        private const int _imageWidth = 1290;
        private const int _imageHeight = 1080;
        private const int _imageQuality = 30;

        public static void ExtentReportInit() {
            try {
                EnsureTestResultDirectoryExists();

                var htmlReporter = new ExtentHtmlReporter(_testResultPath) {
                    Config = {
                        ReportName = "Automation Status Report",
                        DocumentTitle = "Automation Status Report"
                    }
                };
                htmlReporter.Start();

                _extentReports = new ExtentReports();
                _extentReports.AttachReporter(htmlReporter);
                AddSystemInfo();
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao inicializar o relatório: {ex.Message}");
            }
        }

        private static void EnsureTestResultDirectoryExists() {
            if (!Directory.Exists(_testResultPath)) {
                Directory.CreateDirectory(_testResultPath);
            }
        }

        private static void AddSystemInfo() {
            _extentReports.AddSystemInfo("Application", _applicationName);
            _extentReports.AddSystemInfo("Browser", _browserName);
            _extentReports.AddSystemInfo("OS", _osName);
        }

        public static void ExtentReportTearDown() {
            try {
                _extentReports.Flush();
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao finalizar o relatório: {ex.Message}");
            }
        }

        public static string TakeScreenshot(IWebDriver driver, ScenarioContext scenarioContext) {
            try {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

                using var memoryStream = new MemoryStream(screenshot.AsByteArray);
                using var image = new MagickImage(memoryStream);

                AdjustImage(image);

                using var outputStream = new MemoryStream();
                image.Write(outputStream);

                return Convert.ToBase64String(outputStream.ToArray());
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao capturar o screenshot: {ex.Message}");
                return string.Empty;
            }
        }

        private static void AdjustImage(MagickImage image) {
            image.Resize(_imageWidth, _imageHeight);
            image.Quality = _imageQuality;
            image.Strip();
            image.Format = MagickFormat.WebP;
        }
    }
}
