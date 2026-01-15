using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
    public class EnemyBaseState : IState
    {

        protected readonly EnemyAI enemyAI;

        public EnemyBaseState(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }
        public virtual void FixedUpdate()
        {
            //noop
        }

        public virtual void OnEnter()
        {
            //noop
        }

        public virtual void OnExit()
        {
            //noop
        }

        // Start is called before the first frame update

        public virtual void Update()
        {
            //noop
        }
    }


}