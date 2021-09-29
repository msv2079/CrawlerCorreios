using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace CrawlerCorreios
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Preencha o CEP apenas com números");

				var cep = Console.ReadLine();

				if (cep.Length != 8)
				{
					throw new Exception("Ainda não conheço CEP's com esta quantidade de números!");
				}

				if (!double.TryParse(cep, out var dblTesteCep))
				{
					throw new Exception("Preencha o CEP apenas com números");
				}

				cep = cep.Insert(5, "-");

				using (var _driver = new ChromeDriver())
				{
					_driver.Navigate().GoToUrl("https://buscacepinter.correios.com.br/app/endereco/index.php");

					var inputCep = FindElement(_driver, By.XPath("//input[@name='endereco']"), 5);

					inputCep.SendKeys(cep);

					var buttonBuscar = FindElement(_driver, By.Id("btn_pesquisar"), 5);

					buttonBuscar.Click();

					var tabelaResultado = FindElement(_driver, By.Id("resultado-DNEC"), 5);

					Thread.Sleep(TimeSpan.FromSeconds(1));

					var tds = tabelaResultado.FindElements(By.TagName("td"));

					Console.Clear();

					Console.WriteLine($"Logradouro/Nome: {tds[0].Text}");
					Console.WriteLine($"Bairro/Distrito: {tds[1].Text}");
					Console.WriteLine($"Localidade/UF: {tds[2].Text}");
					Console.WriteLine($"CEP: {tds[3].Text}");

					_driver.Close();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.ReadKey();
		}

		private static IWebElement FindElement(IWebDriver driver, By by, int timeoutInSeconds)
		{
			if (timeoutInSeconds > 0)
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

				return wait.Until(drv => drv.FindElement(by));
			}

			return driver.FindElement(by);
		}
	}
}
