using System;

using UnityEngine;

using DS.Enums;

namespace DS.Nodes
{
    public class DSBranchNode : DSBaseNode
    {
        public new string type = "Branch";


        public Condition[] conditions;

        [Input] public DSBaseNode input;
        [Output(connectionType = ConnectionType.Override)] public DSBaseNode pass;
        [Output(connectionType = ConnectionType.Override)] public DSBaseNode fail;


        [Serializable]
        public class Condition : SerializableCallback<bool> { }
    }
}
