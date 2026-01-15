using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
    public abstract class BaseState : IState
    {

        protected readonly FirstPersonController player;


        protected BaseState(FirstPersonController player)
        {
            this.player = player;
        }
        public virtual void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }

}