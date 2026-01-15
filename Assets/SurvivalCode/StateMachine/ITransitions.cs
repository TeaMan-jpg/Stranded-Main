using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
   

    public interface ITransitions {
        IState TargetState { get; }
        IPredicate Condition { get; }
    }

}
