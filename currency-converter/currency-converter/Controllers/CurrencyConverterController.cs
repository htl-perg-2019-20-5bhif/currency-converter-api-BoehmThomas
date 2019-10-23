using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace currency_converter.Controllers
{
    public class ExchangeRates
    {
        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public ExchangeRates (string Line)
        {
            string[] item = Line.Split(",");
            this.Currency = item[0];
            this.Rate = Decimal.Parse(item[1], CultureInfo.InvariantCulture);
        }
    }


    public class Prices
    {
        public string Description { get; set; }

        public string Currency { get; set; }

        public decimal Price { get; set; }

        public Prices(string Line)
        {
            string[] item = Line.Split(",");
            this.Description = item[0];
            this.Currency = item[1];
            this.Price = Decimal.Parse(item[2], CultureInfo.InvariantCulture);
        }
    }


    [ApiController]
    [Route("api/products")]
    public class CurrencyConverterController : ControllerBase
    {
        readonly List<ExchangeRates> exchangeRates = System.IO.File.ReadAllLines("ExchangeRates.csv").Skip(1).Select(line => new ExchangeRates(line)).ToList();
        readonly List<Prices> prices = System.IO.File.ReadAllLines("Prices.csv").Skip(1).Select(line => new Prices(line)).ToList();


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(prices);
        }

        [HttpGet]
        [Route("{product}/price/{currency}")]
        public IActionResult GetProduct(string product, string currency)
        {
            Prices p = null;
            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i].Description.ToLower().Contains(product.ToLower()) || prices[i].Description.ToLower().Contains(product.ToLower()))
                {
                    p = prices[i];
                }
            }

            if (p != null)
            {
                if (p.Currency.ToLower().Equals(currency.ToLower()))
                {
                    return Ok("price in " + currency + ": " + p.Price);
                } else
                {
                    decimal price = p.Price;

                    if(!p.Currency.ToLower().Equals("eur".ToLower()))
                    {
                        price = PriceToEur(p);
                    }
                    
                    if (!currency.ToLower().Equals("eur".ToLower()))
                    {
                        price = PriceFromEur(price, currency);
                    }
                    return Ok("price in " + currency + ": " + price);
                }
            }
            return BadRequest("Invalid or missing name");
        }

        public decimal PriceFromEur(decimal price, string currency)
        {
            for (int i = 0; i < exchangeRates.Count; i++)
            {
                if (exchangeRates[i].Currency.ToLower().Equals(currency.ToLower()))
                {
                    return Math.Round(price * exchangeRates[i].Rate, 2);
                }
            }

            return 0.0m;
        }

        public decimal PriceToEur(Prices p)
        {
            for (int j = 0; j < exchangeRates.Count; j++)
            {
                if (exchangeRates[j].Currency.ToLower().Equals(p.Currency.ToLower()))
                {
                    return Math.Round(p.Price / exchangeRates[j].Rate, 2);
                }
            }

            return 0.0m;
        }

    }
}
