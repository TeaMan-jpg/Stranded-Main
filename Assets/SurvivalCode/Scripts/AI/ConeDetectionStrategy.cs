using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Platformers {
    public class ConeDetectionStrategy : IDetectionStrategy
    {
        readonly float detectionAngle;
        readonly float detectionRadius;
        readonly float innerDetectionRadius;

        public ConeDetectionStrategy(float angle, float radius, float innerRadius)
        {
            this.detectionAngle = angle;
            this.detectionRadius = radius;
            this.innerDetectionRadius = innerRadius;
        }

        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            if (timer.IsRunning) return false;

            var directionToPlayer = (player.position - detector.position);
            float angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);
            if (!(angleToPlayer < detectionAngle / 2f || directionToPlayer.magnitude < detectionRadius) && !(directionToPlayer.magnitude < innerDetectionRadius))
            {
                return false;
            }
            timer.Start();
            return true;
        }
    }

}