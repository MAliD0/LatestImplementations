using DS.Nodes;
using System;
using System.Collections.Generic;
using DS.Enums;
using UnityEngine;
using DS.Graph;

namespace DS.Nodes
{
    [NodeWidth(500)]
    public class DSChoiceNode : DSBaseNode
    {
        [Input] public DSBaseNode input;
        [TextArea]public string Question;
        public string characterAndMood;
        [Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<Answer> answers = new List<Answer>();

        public override string GetString()
        {
            return characterAndMood + "^" + Question;
        }

        public override string getType()
        {
            return "Choice";
        }

        public void AddNewOption()
        {
            this.AddDynamicOutput(typeof(int), fieldName: "myDynamicOutput");
        }


    }
    [Serializable]
    public class Answer
    {
        public string answer;
    }
}
