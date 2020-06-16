using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json;

namespace vk_login_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;

                var options = new ChromeOptions();
                options.AddArgument("--window-position= -32000,-32000");
                options.AddArgument("headless");
                options.BinaryLocation = @"ContraCity_Data\Files\VK_AUTH\Chrome\chrome.exe";
                using (var driver = new ChromeDriver(driverService, options))
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    driver.Navigate().GoToUrl("https://vk.com/app3636515");
                    var userNameField = driver.FindElementById("quick_email");
                    var userPasswordField = driver.FindElementById("quick_pass");
                    var loginButton = driver.FindElementById("quick_login_button");

                    userNameField.SendKeys(textBox1.Text);
                    userPasswordField.SendKeys(textBox2.Text);
                    loginButton.Click();

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
                    if (IsElementPresent(By.Id("login_message")))
                    {
                        var EDF = new Error();
                        EDF.ShowDialog();
                    }
                    bool IsElementPresent(By by)
                    {
                        try
                        {
                            driver.FindElement(by);
                            return true;
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    }
                    if (IsElementPresent(By.Id("authcheck_code")))
                    {
                        var EDF = new EnterCodeForm();
                        EDF.ShowDialog();
                        var TauthField = driver.FindElementById("authcheck_code");
                        var authBtn = driver.FindElementById("login_authcheck_submit_btn");
                        TauthField.SendKeys(EDF.Code);
                        authBtn.Click();
                    }
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                    IWebElement firstResult = wait.Until(ExpectedConditions.ElementExists(By.Id("fXD")));
                    string url = driver.FindElement(By.XPath("//iframe[@id='fXD']")).GetAttribute("src");


                    JObject output = new JObject(
                        new JProperty("url", url));
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\VK_login.json", output.ToString());
                    using (StreamWriter file = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + @"\VK_login.json"))
                    using (JsonTextWriter writer = new JsonTextWriter(file))
                    {
                        output.WriteTo(writer);
                    }
                    Console.ReadLine();
                    Task.Delay(2000);
                    driver.Close();
                }
                var good = new Good();
                good.ShowDialog();
            }
            catch (Exception error)
            {
            }
        }    

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
