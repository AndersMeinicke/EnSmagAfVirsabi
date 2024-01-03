using UnityEngine;
using MyBox;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Virsabi
{
    [ExecuteInEditMode]
    public class LookAtConstraint : MonoBehaviour
    {
        [SerializeField]
        private bool useMainCamera;

        [SerializeField, ConditionalField(nameof(useMainCamera))]
        private bool applyMainInUpdate;

        [SerializeField, ConditionalField(nameof(useMainCamera), true)]
        public Transform lookAtTarget;

        [ConditionalField(nameof(lookAtTarget))]
        public UpdateMethod updateMethod;

        [ConditionalField(nameof(lookAtTarget))]
        public Vector3 offset;

        [ConditionalField(nameof(lookAtTarget))]
        public bool active, X = true, Y = true, Z = true;

        //TODO: Implement lerp
        [SerializeField, HideInInspector]
        private bool Lerp;

        //TODO: Implement lerp
        [ConditionalField(nameof(Lerp))]
        public float Damp;

        [Foldout("Debug", true)]

        [SerializeField]
        private bool activeInEditor, onlyActiveInEditor;

        [SerializeField, ReadOnly]
        private Vector3 initRotation;

        public Quaternion resultQuaternion { get; private set; }

        [ButtonMethod]
        public void SetRest()
        {
            initRotation = transform.rotation.eulerAngles;
        }

        private void OnValidate()
        {
            if (useMainCamera && Camera.main != null)
                lookAtTarget = Camera.main.transform;
        }

        private void Start()
        {
            if (useMainCamera && lookAtTarget == null)
                lookAtTarget = Camera.main.transform;
        }

        void Update()
        {
            if (applyMainInUpdate)
                lookAtTarget = Camera.main.transform;

            if (updateMethod != UpdateMethod.update || !active)
                return;
            if (!Application.isPlaying && !activeInEditor)
                return;
            Sync();
        }

        void FixedUpdate()
        {
            if (updateMethod != UpdateMethod.fixedUpdate || !active)
                return;
            if (!Application.isPlaying && !activeInEditor)
                return;
            Sync();
        }

        void LateUpdate()
        {
            if (updateMethod != UpdateMethod.lateUpdate || !active)
                return;
            if (!Application.isPlaying && !activeInEditor)
                return;
            Sync();
        }

        private void Sync()
        {
            if (!lookAtTarget)
                return;

            if (onlyActiveInEditor && Application.isPlaying)
                return;

            Quaternion lookAtRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position) * Quaternion.Euler(offset);

            if (Y && X && Z)
            {
                resultQuaternion = lookAtRotation;
                transform.rotation = resultQuaternion;
                return;
            }

            //TODO: Not working correctly yet...
            Vector3 rot = initRotation;

            if (X)
            {
                rot.x = lookAtRotation.x;
            }
            if (Y)
            {
                rot.y = lookAtRotation.y;
            }
            if (Z)
            {
                rot.z = lookAtRotation.z;
            }

            resultQuaternion = Quaternion.Euler(rot);
            transform.rotation = resultQuaternion;
        }
    }

}