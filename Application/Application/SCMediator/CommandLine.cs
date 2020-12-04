using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Application.SCMediator {
    [Serializable]
    public class CommandLine {
        public string Command { get; set; }
        public string json { get; set; }
    }
}
