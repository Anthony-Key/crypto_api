using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.CustomProperties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PortfolioApi.Controllers.Models;
using System.Text;
using System.Text.Json;

namespace CryptoPriceTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IConfiguration _configuration;

        private static string containerName = "recipes";
        private static string blobName = "data.json";
        public string connectionString = "";

        public RecipeController(ILogger<RecipeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration= config;
            connectionString = _configuration.GetConnectionString("storageKey");
        }

        [HttpGet("{recipeName}")]
        public async Task<string> SearchRecipes(string recipeName)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://www.themealdb.com/api/json/v1/1/search.php?s={recipeName}");
            var jsonString = await response.Content.ReadAsStringAsync();
            return jsonString.ToString();
        }

        [HttpGet(Name = "GetRecipes")]
        public string GetRecipes()
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            BlobDownloadInfo blobInfo = blobClient.Download();

            using (StreamReader reader = new StreamReader(blobInfo.Content))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        [HttpPost(Name = "CreateRecipe")]
        public string CreateRecipe([FromBody] RecipeRoot recipe)
        {
            if (recipe == null)
            {
                return "Malformed data.";
            }

            var blobData = GetBlobData();

            using (StreamReader reader = new StreamReader(blobData.Item1.Content))
            {
                string content = reader.ReadToEnd();
                var recipes = JsonSerializer.Deserialize<List<RecipeRoot>>(content);
                recipes?.Add(recipe);

                string saveBlobData = JsonSerializer.Serialize(recipes);
                byte[] byteArray = Encoding.UTF8.GetBytes(saveBlobData);
                MemoryStream stream = new MemoryStream(byteArray);
                blobData.Item2.Upload(stream, true);
                stream.Dispose();
            }

            return "Success";
        }

        public Tuple<BlobDownloadInfo, BlobClient> GetBlobData()
        {
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            return new Tuple<BlobDownloadInfo, BlobClient>(blobClient.Download(), blobClient);
        }
    }

    public class Player
        {
            public string Name { get; set; }
            public string? WalletAddress { get; set; }
        }
    }