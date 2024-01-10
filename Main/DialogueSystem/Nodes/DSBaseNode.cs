using DS.Graph;
using XNode;
using DS.Enums;
namespace DS.Nodes
{
    public abstract class DSBaseNode : Node
    {
        public virtual void Trigger()
        {
            DialogueGraph DSGraph = graph as DialogueGraph;
            DSGraph.textData?.Invoke(GetString().Split('^')[1]);
        }

        public virtual string getType()
        {
            return "";
        }

        public virtual string GetString()
        {
            return null;
        }

        // Use this for initialization
        protected override void Init()
        {
            base.Init();

        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }
    }
}
	
