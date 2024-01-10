
using UnityEngine;
using DS.Enums;

namespace DS.Nodes
{
    public class DSLineNode : DSBaseNode
    {
        [Input] public DSBaseNode input;
        [Output(connectionType = ConnectionType.Override)] public DSBaseNode output;

        [TextArea]
        public string text;
        public string characterAndMood;

        public override string GetString()
        {
            return characterAndMood + "^"+ text;
        }

        public override string getType()
        {
            return "Line";
        }
    }
}
