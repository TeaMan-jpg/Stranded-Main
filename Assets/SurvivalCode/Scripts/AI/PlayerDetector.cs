using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;
namespace Platformers {

    public class PlayerDetector : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] float detectionRadius = 10f;
        [SerializeField] float detectionAngle = 60f;
        [SerializeField] float innerDetectionRadius = 5f;
        [SerializeField] float detectionCooldown = 1f;


        public Transform playerTransform { get; private set; }
        CountdownTimer detectionTimer;

        IDetectionStrategy detectionStrategy;


        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("FPSController").transform;
        }

        void Start()
        {

            //LoadSettings();
            playerTransform = GameObject.FindGameObjectWithTag("FPSController").transform;
            Debug.Log(playerTransform);
            detectionTimer = new CountdownTimer(detectionCooldown);
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void LoadSettings()
        {
            var settings = DifficultyManager.Instance.currentDifficulty;
            detectionRadius = settings.detectionRange;
            detectionAngle = settings.detectionAngle;
            innerDetectionRadius = settings.innerDetectionRadius;
            detectionCooldown = settings.detectionCooldown;
        }


        void Update() => detectionTimer.Tick(Time.deltaTime);

        public bool CanDetectPlayer() {
            return detectionTimer.IsRunning || detectionStrategy.Execute(playerTransform, transform, detectionTimer);

        }

        public void SetDectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2f, 0) * transform.forward * detectionRadius;
            Vector3 backConeDirection = Quaternion.Euler(0, -detectionAngle / 2f, 0) * transform.forward * detectionRadius;

            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backConeDirection);
        }
    }



}