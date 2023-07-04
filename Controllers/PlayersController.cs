using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace CryptoPriceTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : Controller
    {
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUsers")]
        public string GetUsers()
        {
            string containerName = "playercontainer";
            string blobName = "data.json";

            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            BlobDownloadInfo blobInfo = blobClient.Download();

            using (StreamReader reader = new StreamReader(blobInfo.Content))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        [HttpPost(Name = "JoinUser")]
        public string JoinUser([FromBody] Player player)
        {
            if (player == null)
            {
                return "Malformed data.";
            }

            if (string.IsNullOrEmpty(player.Name) || player.Name.Length > 20)
            {
                return "No name was input or characters exceeded 20.";
            }

            string containerName = "playercontainer";
            string blobName = "data.json";

            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            BlobDownloadInfo blobInfo = blobClient.Download();

            using (StreamReader reader = new StreamReader(blobInfo.Content))
            {
                string content = reader.ReadToEnd();
                var players = JsonSerializer.Deserialize<List<Player>>(content);
                players ??= new List<Player>();
                players.Add(player);
                string saveBlobData = JsonSerializer.Serialize(players);
                byte[] byteArray = Encoding.UTF8.GetBytes(saveBlobData);
                MemoryStream stream = new MemoryStream(byteArray);
                blobClient.Upload(stream, true);
                stream.Dispose();
            }

            return "Success";
        }
    }

    public class Player
        {
            public string Name { get; set; }
            public string? WalletAddress { get; set; }
        }
    }