using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
        // Start is called before the first frame update
    }
}
