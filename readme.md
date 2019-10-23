# Currency Converter

## Introduction

Your job is to write a Web API with which prices of products can be converted into different currencies.

Given are two CSV (comma-separated values) files:

* One contains exchange rates.
* The other one contains products and their prices in a given currency.

Users can use your Web API to get the price of a product. If necessary, your program has to convert the product's price based on the exchange rate table.

## CSV Files

### General Information

* Line ending: CRLF
* Ignore the first line (column headers)
* Ignore empty lines
* You can use a NuGet package for CSV parsing, but I would *strongly recommend* to not do that.

### Exchange Rate Table

Your program has to read the exchange rate file from [https://cddataexchange.blob.core.windows.net/data-exchange/htl-homework/ExchangeRates.csv](https://cddataexchange.blob.core.windows.net/data-exchange/htl-homework/ExchangeRates.csv). It contains exchange rates against EUR. Here is its content:

```txt
Currency,Rate
USD,1.10
CHF,1.10
GBP,0.86
TRL,6.50
```

The exchange rate e.g. *USD,1.10* means that you get 1.10 USD for one EUR, *GBP,0.86* means that you get 0.86 GBP for one EUR, etc.

### Product Prices

Your program has to read product data from [https://cddataexchange.blob.core.windows.net/data-exchange/htl-homework/Prices.csv](https://cddataexchange.blob.core.windows.net/data-exchange/htl-homework/Prices.csv). Here is its content:

```txt
Description,Currency,Price
Car,USD,30000
Bike,CHF,800
Book,TRL,20
Chocolate,EUR,3.5
```

The column *Price* contains the price in the currency contained in column *Currency*.

## Requirements

* Use *ASP.NET Core 3* and C#.

* Use the proper mechanism for retrieving a `HttpClient` instance in ASP.NET Core.

* You do *not* have to implement error handling. You can assume that the CSV files can be successfully read and that they contain meaningful values. You can assume that meaningful parameters are given. If not, it is ok if your app crashes with an unhandled exception.

* Put the business logic (parsing of the CSV files, currency conversion) in a reusable *.NET Standard 2.1* class library. The class library *must not* contain any input or output (e.g. sending HTTP requests, reading files, writing to console).

* Read the CSV files into appropriate in-memory data structures. It is up to you to decide which data structures make most sense.
  * **Tip:** For exchange rates and monetary values, use the C# data type `decimal`
  * **Tip:** The CSV files contain English number format. You can parse it using `decimal.Parse(numberAsString, CultureInfo.InvariantCulture)`

* Implement those part of the CSV parsing logic that is common for both CSV files only once. Avoid duplicating this logic.

* Your Web API receives the name of a product in the URL path and a target currency as a URL query parameter (e.g. *https://localhost:5001/api/products/Car/price?targetCurrency=EUR*). The target currency is always *EUR* (earn extra points if you support other target currencies, too; details see below). Examples:
  * *https://localhost:5001/api/products/Car/price?targetCurrency=EUR* leads to *{ "price": 27272.73 }*
  * *https://localhost:5001/api/products/Book/price?targetCurrency=EUR* leads to *{ "price": 3.08 }*
  * *https://localhost:5001/api/products/Chocolate/price?targetCurrency=EUR* leads to *{ "price": 3.50 }*

## Extra Points

Notify Mr. Stropek via GitHub issue if you think you have earned extra points.

### Rounded Results

All numbers returned by the API should be *rounded* to 2 decimal places (e.g. 27272,73). It is part of the homework to find out how to round decimal values.

### Cross-Rate Calculation

Your app can convert prices into any target currency (e.g. *https://localhost:5001/api/products/Car/price?targetCurrency=GBP*). If the target currency is not EUR, you have to internally calculate from the original currency into EUR, and then from EUR to the target currency. Examples:

* *https://localhost:5001/api/products/Car/price?targetCurrency=GBP* leads to *{ "price": 23454.55 }*
* *https://localhost:5001/api/products/Bike/price?targetCurrency=GBP* leads to *{ "price": 800.00 }*
