//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using static Unity.VisualScripting.Metadata;

//namespace Platformers
//{

//    public class Sequences : NodeBehaviourTree
//    {
//        public Sequences(string name) : base(name)
//        {
//        }
//        public override NodeState Process()
//        {
//            while (currentChild < children.Count) {
//                switch (children[currentChild].Process())
//                {
//                    case NodeState.FAILURE:
//                        Reset();
//                        return NodeState.FAILURE;
//                    case NodeState.RUNNING:
//                        return NodeState.RUNNING;
//                    default:
//                        currentChild++;
//                        return currentChild == children.Count ? NodeState.SUCCESS : NodeState.RUNNING;
//                }
                
//            }

//            Reset();
//            return NodeState.SUCCESS;
//        }
//    }

//    public class BehaviourTree : NodeBehaviourTree
//    {

//        public BehaviourTree(string name) : base(name)
//        {
//        }

//        public override NodeState Process()
//        {
//            while (currentChild < children.Count) {
//                var status = children[currentChild].Process();
//                if (status != NodeState.SUCCESS) {
//                    return status;
                
//                }
//                currentChild++;
//            }
//            return NodeState.SUCCESS;
//        }
//    }

    
//    public class Leaf : NodeBehaviourTree
//    {
//        readonly Strategies.IStrategy strategy;

//        public Leaf(string name, Strategies.IStrategy strategy) : base(name) {
//            this.strategy = strategy;
//        }

//        public override NodeState Process() => strategy.Process();


//        public override void Reset() => strategy.Reset();
//    }

//    public class NodeBehaviourTree : MonoBehaviour
//    {
//        // Start is called before the first frame update
//        public enum NodeState
//        {
//            RUNNING,
//            SUCCESS,
//            FAILURE
//        }

//        public readonly List<NodeBehaviourTree> children = new List<NodeBehaviourTree>();
//        protected int currentChild;

//        // Add a parameterless constructor for MonoBehaviour compatibility
//        //public NodeBehaviourTree() : this("Node") { }

//        // Existing constructor
//        public NodeBehaviourTree(string name) {
//            this.name = name;
//        }

//        public void AddChild(NodeBehaviourTree child) => children.Add(child);

//        public virtual NodeState Process() {
//            currentChild = 0;
//            return children[currentChild].Process();
//        }

//        public virtual void Reset() {
//            currentChild = 0;
//            foreach (var child in children) {
//                child.Reset();
//            }
//        }




//    }
//}