//using Platformers;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class Strategies : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public interface IStrategy
//    {
//        NodeBehaviourTree.NodeState Process();
//        void Reset() {
//            //Noop
//        }
//    }

//    public class ActionStrategy : IStrategy
//    {
//        readonly Action action;
//        public ActionStrategy(Action action)
//        {
//            this.action = action;
//        }
//        public NodeBehaviourTree.NodeState Process()
//        {
//            action();
//            return NodeBehaviourTree.NodeState.SUCCESS;
//        }
//    }

//    public class Condition : IStrategy
//    {
//        readonly Func<bool> condition;
//        public Condition(Func<bool> condition)
//        {
//            this.condition = condition;
//        }

        

//    }

//    public class PatrolStrategy: IStrategy
//    {
//        readonly Transform entity;
//        readonly NavMeshAgent agent;
//        readonly List<Transform> patrolPoints;
//        readonly float patrolSpeed;
//        int currentIndex;
//        bool isPathCalculated;
//        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
//        {
//            this.entity = entity;
//            this.agent = agent;
//            this.patrolPoints = patrolPoints;
//            this.patrolSpeed = patrolSpeed;
//            currentIndex = 0;
//        }

//        NodeBehaviourTree.NodeState IStrategy.Process()
//        {
//            if (currentIndex == patrolPoints.Count) return NodeBehaviourTree.NodeState.SUCCESS;
//            var targetPoint = patrolPoints[currentIndex];
//            agent.SetDestination(targetPoint.position);
//            entity.LookAt(targetPoint);

//            if (isPathCalculated && agent.remainingDistance < 0.1f) {
//                currentIndex++;
//                isPathCalculated = false;

//            }

//            if (agent.pathPending) {
//                isPathCalculated = true;

//            }

//            return NodeBehaviourTree.NodeState.RUNNING;
//        }

//        public void Reset()
//        {
//            currentIndex = 0;
//        }

//    }
//}
