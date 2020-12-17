using System;

namespace Application.SCMediator
{/// <summary>
 /// Class containing a command to be initialized when sending it to Tier 3 and fields to contain data if needed
 /// </summary>
    [Serializable]
    public class CommandLine
    {
        public string Command { get; set; }
        public string variableUser { get; set; }
        public string variableChatroom { get; set; }
        public string SpecificOrder { get; set; }
    }
}