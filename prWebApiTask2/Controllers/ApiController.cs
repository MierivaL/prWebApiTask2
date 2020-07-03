using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Linq;

namespace prWebApiTask2.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ApiController : Controller
    {
        MongoClient client;
        IMongoDatabase database;
        public ApiController()
        {
            client = new MongoClient("mongodb+srv://prUser:prPass@cluster0.37pt1.mongodb.net/<dbname>?retryWrites=true&w=majority");
            database = client.GetDatabase("users");
        }

        [HttpGet]
        public async Task<JsonResult> GetUser()
        {
            var collection = database.GetCollection<User>("_id");
            var documents = await (await collection.FindAsync(new BsonDocument())).ToListAsync();
            if (documents.Count == 0)
                throw new Exception("Нет пользователей в БД.");

            return Json(documents);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetUserById(string Id)
        {
            var collection = database.GetCollection<User>("_id");
            if (string.IsNullOrEmpty(Id))
                throw new Exception("Указан пустой Id");
            var documents = await (await collection.FindAsync<User>(x => x._id == Id)).ToListAsync();
            if (documents.Count == 0)
                throw new Exception("Пользователь с указанным ID не найден.");
            return Json(documents.First());
        }

        // POST api
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] string json)
        {
            WebFormUser info = JsonSerializer.Deserialize<WebFormUser>(json);
            var collection = database.GetCollection<BsonDocument>("_id");
            if (collection.CountDocumentsAsync(new BsonDocument("_id", info.Id)).Result == 0)
            {
                User user = new User(info.Id, new UserInfo(info.RealName, DateTime.Parse(info.BirthDate), info.PhoneNumber));
                var document = user.ToBsonDocument();
                await collection.InsertOneAsync(document);
            }
            else throw new Exception("Такой пользователь уже существует");
            return Redirect("~/Api?Id=" + info.Id);
        }

        // PUT api
        [HttpPut]
        public async Task<ActionResult> Put([FromForm] string json)
        {
            WebFormUser info = JsonSerializer.Deserialize<WebFormUser>(json);
            var collection = database.GetCollection<BsonDocument>("_id");
            if (collection.CountDocumentsAsync(new BsonDocument("_id", info.Id)).Result != 0)
            {
                User user = new User(info.Id, new UserInfo(info.RealName, DateTime.Parse(info.BirthDate), info.PhoneNumber));
                var document = await collection.ReplaceOneAsync(new BsonDocument("_id", info.Id),
                    user.ToBsonDocument());
            }
            else throw new Exception("Такого пользователя не существует");
            return Redirect("~/Api?Id="/* + info.PhoneNumber*/);
        }
    }
}