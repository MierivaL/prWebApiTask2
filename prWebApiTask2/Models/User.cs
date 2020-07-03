using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prWebApiTask2
{
    public class User
    {
        private string __id;
        [BsonId]
        public string _id
        {
            get { return __id; }
            set
            {
                if (value.Equals("") || value.Contains(' ')) throw new Exception("Не введен ID пользователя.");
                else __id = value;
            }
        }
        public UserInfo Info { get; set; }

        public User(string Id, UserInfo Info) { this._id = Id; this.Info = Info; }

        public User() { }
    }
}
