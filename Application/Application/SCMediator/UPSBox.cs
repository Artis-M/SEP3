using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Application.SCMediator {
    [Serializable]
    public class UPSBox {
        public string type { get; set; }
        public ObjectId user;
        public string JSonThing;
    }
}
