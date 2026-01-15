using System.Collections.Generic;

namespace Platformers
{
    public enum NodeState { RUNNING, SUCCESS, FAILURE }

    public abstract class Node
    {
        protected NodeState state;
        public List<Node> children = new List<Node>();

        public abstract NodeState Evaluate();
    }

    // Selector: Runs until one child SUCCEEDS
    public class Selector : Node
    {
        public Selector(List<Node> children) => this.children = children;
        public override NodeState Evaluate()
        {
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE: continue;
                    case NodeState.SUCCESS: return state = NodeState.SUCCESS;
                    case NodeState.RUNNING: return state = NodeState.RUNNING;
                }
            }
            return state = NodeState.FAILURE;
        }
    }

    // Sequence: Runs until one child FAILS
    public class Sequence : Node
    {
        public Sequence(List<Node> children) => this.children = children;
        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;
            foreach (var node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE: return state = NodeState.FAILURE;
                    case NodeState.SUCCESS: continue;
                    case NodeState.RUNNING: anyChildRunning = true; continue;
                }
            }
            return state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }
}