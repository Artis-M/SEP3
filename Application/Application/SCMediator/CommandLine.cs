using System;

namespace Application.SCMediator
{
    [Serializable]
    public class CommandLine
    {
        public string Command { get; set; }
        public string variableUser { get; set; }
        public string variableChatroom { get; set; }
        public string SpecificOrder { get; set; }
    }
}