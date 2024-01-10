using DS.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DS.Graph{
    [CreateAssetMenu]
    public class DialogueGraph : NodeGraph
    {
        public DSBaseNode current;
       
        public string nodeType;
       
        public Action check;
        public Action<string> textData;
        public Action dialogueEnd;
        public Action choiceNode;
        public void ContinueDialogue()
        {
            NextNode("output");

            if (nodeType == "Line")
            {
                current.Trigger();
            }
            if (nodeType == "Choice")
            {
                current.Trigger();
                choiceNode?.Invoke();
            }
            if (nodeType == "Exit")
            {
                EndDialogue();
            }
        }
        public void ContinueDialogue(string nodeName)
        {
            NextNode(nodeName);

            if (nodeType == "Line")
            {
                current.Trigger();
            }
            if (nodeType == "Choice")
            {
                current.Trigger();
                choiceNode?.Invoke();
            }
            if (nodeType == "Exit")
            {
                EndDialogue();
            }
        }
        public void StartDialogue()
        {
            foreach (DSBaseNode node in nodes)
            {
                if(node.getType() == "Start")
                {
                    current = node;
                    ContinueDialogue();
                    break;
                }
            }
        }

        public void EndDialogue()
        {
            current = null;
            dialogueEnd?.Invoke();
        }

        public void NextNode(string fieldName)
        {
            NodePort otherPort = current.GetPort(fieldName).Connection;
            if (otherPort != null)
            {
                current = otherPort.node as DSBaseNode;
                nodeType = current.getType();
            }
            else
            {
                otherPort = current.GetPort(fieldName).Connection;
                nodeType = current.getType();
            }
        }

        public string[] GetCurrentSpeaker()
        {
            string dataRaw = current.GetString().Split('^')[0];
            string[] data = dataRaw.Split('/');
            
            string[] str = { "Narrator", "" };

            if (data[0] == "s")
            {
                string[] speaker = { data[1], data[2] };
                return speaker;
            }

            return str;
        }
    }
}
