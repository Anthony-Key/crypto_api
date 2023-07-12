using CryptoPriceTrader.Controllers.Types;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Controllers.Models;
using System.Text.Json;


namespace CryptoPriceTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoPricesController : ControllerBase
    {
        //Hardcoded data for now until I can find an API for this data.
        private static readonly List<Crypto> cryptos = new List<Crypto>
        {
            new Crypto { Name = "btc", EtoroUrlCode = "100000", Description = "Bitcoin is a decentralized cryptocurrency originally described in a 2008 whitepaper by a person, or group of people, using the alias Satoshi Nakamoto. It was launched soon after, in January 2009.", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/1.png"  },
            new Crypto { Name = "xrp", EtoroUrlCode = "100003",  Description = "Launched in 2021, the XRP Ledger (XRPL) is an open-source, permissionless and decentralized technology. Benefits of the XRP Ledger include its low-cost ($0.0002 to transact), speed (settling transactions in 3-5 seconds), scalability (1,500 transactions per second) and inherently green attributes (carbon-neutral and energy-efficient).", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/52.png" },
            new Crypto { Name = "hbar" , EtoroUrlCode = "100061", Description = "Hedera is the most used, sustainable, enterprise-grade public network for the decentralized economy that allows individuals and businesses to create powerful decentralized applications (DApps).", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/4642.png"},
            new Crypto { Name = "doge" , EtoroUrlCode = "100043",  Description = "Dogecoin (DOGE) is based on the popular \"doge\" Internet meme and features a Shiba Inu on its logo. The open-source digital currency was created by Billy Markus from Portland, Oregon and Jackson Palmer from Sydney, Australia, and was forked from Litecoin in December 2013.", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/74.png"},
            new Crypto { Name = "link" , EtoroUrlCode = "100040",  Description = "Founded in 2017, Chainlink is a blockchain abstraction layer that enables universally connected smart contracts. Through a decentralized oracle network, Chainlink allows blockchains to securely interact with external data feeds, events and payment methods, providing the critical off-chain information needed by complex smart contracts to become the dominant form of digital agreement.", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/1975.png"},
            new Crypto { Name = "eos" , EtoroUrlCode = "100022" ,  Description = "The EOS Network is an open-source blockchain platform that prioritizes high performance, flexibility, security, and developer experience. As a third-generation blockchain platform powered by the EOS virtual machine, EOS has an extensible WebAssembly engine for deterministic execution of near fee-less transactions.", CryptoImage = "https://s2.coinmarketcap.com/static/img/coins/64x64/1765.png"}
        };

        [HttpGet(Name = "GetAll")]
        public async Task<List<CryptoDAO>> GetAll()
        {
            List<CryptoDAO> doas = new List<CryptoDAO>();

            foreach (Crypto crypto in cryptos)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"https://candle.etoro.com/candles/asc.json/OneDay/2/{crypto.EtoroUrlCode}");
                var jsonString = await response.Content.ReadAsStringAsync();
                CryptoRoot cryptoData = JsonSerializer.Deserialize<CryptoRoot>(jsonString);

                if(cryptoData == null || cryptoData.Candles.Count == 0) 
                {
                    break;
                }

                CryptoDAO doa = new CryptoDAO()
                {
                    Name = crypto.Name,
                    Price = cryptoData.Candles[0].RangeClose.ToString(),
                    Description = crypto.Description,
                    TokenImage = crypto.CryptoImage
                };

                doas.Add(doa);
            }

            return doas;
        }
    }
}