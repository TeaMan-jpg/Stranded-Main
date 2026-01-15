using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
    public class Transitions : ITransitions {
        public IState TargetState { get; private set; }
        public IPredicate Condition { get; private set; }

        public Transitions(IState targetState, IPredicate condition) {
            TargetState = targetState;
            Condition = condition;
        }

    }
}
