using UnityEngine;
using DS.Enums;

namespace DS.Nodes
{
    public class DSExitNode : DSBaseNode
    {
        public override string getType()
        {
            return "Exit";
        }

        private void Reset()
        {
            base.name = "exit";
        }
        [Input] public DSBaseNode baseNode;
    }
}
