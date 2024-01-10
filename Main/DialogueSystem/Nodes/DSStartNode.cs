using DS.Enums;

namespace DS.Nodes
{
    public class DSStartNode : DSBaseNode
    {
        public new string type = "Start";
        [Output(connectionType = ConnectionType.Override)] public DSBaseNode output;

        public override void Trigger()
        {
            return;
        }

        private void Reset()
        {
            name = "start";
        }
        public override string getType()
        {
            return "Start";
        }

    }
}
