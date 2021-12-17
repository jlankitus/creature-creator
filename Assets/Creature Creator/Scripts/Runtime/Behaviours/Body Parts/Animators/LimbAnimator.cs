﻿// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(LimbConstructor))]
    public class LimbAnimator : BodyPartAnimator
    {
        #region Fields
        protected Transform target, limb;
        protected ChainIKConstraint limbIK; //protected TwoBoneIKConstraint limbIK;
        
        protected Vector3[] defaultBonePositions;
        protected Quaternion defaultExtremityRotation;
        protected bool hasCapturedDefaults;
        #endregion

        #region Properties
        public LimbConstructor LimbConstructor => BodyPartConstructor as LimbConstructor;
        public LimbAnimator FlippedLimb => Flipped as LimbAnimator;

        public Transform Target => target;
        public Vector3[] DefaultPositions => defaultBonePositions;
        #endregion

        #region Methods
        protected virtual void LateUpdate()
        {
            HandleTarget();
        }
        protected virtual void OnDestroy()
        {
            if (limbIK != null)
            {
                Destroy(limbIK.gameObject);
            }
        }

        public override void Setup(CreatureAnimator creatureAnimator)
        {
            base.Setup(creatureAnimator);

            limb = new GameObject("Limb").transform;
            limb.SetParent(creatureAnimator.Rig, false);

            target = new GameObject("Target").transform;
            target.SetParent(limb, false);

            //if (LimbConstructor.Bones.Length == 3)
            //{
            //    limbIK = limb.gameObject.AddComponent<TwoBoneIKConstraint>();
            //    limbIK.Reset();

            //    limbIK.data.root = LimbConstructor.Bones[0];
            //    limbIK.data.mid = LimbConstructor.Bones[1];
            //    limbIK.data.tip = LimbConstructor.Bones[2];
            //    limbIK.data.target = target;
            //    limbIK.data.targetRotationWeight = 0;
            //}
            //else
            {
                limbIK = limb.gameObject.AddComponent<ChainIKConstraint>();
                limbIK.Reset();

                limbIK.data.root = LimbConstructor.Bones[0];
                limbIK.data.tip = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1];
                limbIK.data.target = target;
                limbIK.data.tipRotationWeight = 0;
            }

            LimbConstructor.OnAttach += delegate
            {
                limb.name = $"Limb ({name})";
                FlippedLimb.limb.name = $"Limb ({FlippedLimb.name})";
            };
        }

        public virtual void Restructure(bool isAnimated)
        {
            // Limb
            for (int i = 1; i < LimbConstructor.Bones.Length; i++)
            {
                LimbConstructor.Bones[i].SetParent(isAnimated ? LimbConstructor.Bones[i - 1] : LimbConstructor.Root);
            }

            // Connected Extremity
            if (LimbConstructor.ConnectedExtremity != null)
            {
                LimbConstructor.ConnectedExtremity.transform.SetParent(isAnimated ? LimbConstructor.Extremity : CreatureAnimator.CreatureConstructor.Bones[LimbConstructor.AttachedLimb.boneIndex]);
            }
        }
        public virtual void Reposition(bool isAnimated)
        {
            if (isAnimated)
            {
                CaptureDefaults();
            }
            else if (hasCapturedDefaults)
            {
                RestoreDefaults();
            }
        }
        private void CaptureDefaults()
        {
            // Limb
            defaultBonePositions = new Vector3[LimbConstructor.Bones.Length];
            for (int i = 0; i < defaultBonePositions.Length; i++)
            {
                defaultBonePositions[i] = CreatureAnimator.CreatureConstructor.Body.InverseTransformPoint(LimbConstructor.Bones[i].position);
            }

            // Connected Extremity
            if (LimbConstructor.ConnectedExtremity != null)
            {
                defaultExtremityRotation = Quaternion.Inverse(CreatureAnimator.CreatureConstructor.Body.rotation) * LimbConstructor.ConnectedExtremity.transform.rotation;
            }

            hasCapturedDefaults = true;
        }
        private void RestoreDefaults()
        {
            // Limb
            for (int i = 0; i < defaultBonePositions.Length; i++)
            {
                LimbConstructor.Bones[i].position = CreatureAnimator.CreatureConstructor.Body.TransformPoint(defaultBonePositions[i]);
            }

            // Connected Extremity
            if (LimbConstructor.ConnectedExtremity != null)
            {
                LimbConstructor.ConnectedExtremity.transform.rotation = CreatureAnimator.CreatureConstructor.Body.rotation * defaultExtremityRotation;
            }

            hasCapturedDefaults = false;
        }

        public void HandleTarget()
        {
            if (CreatureAnimator.IsAnimated)
            {
                target.rotation = LimbConstructor.Bones[LimbConstructor.Bones.Length - 1].rotation;
            }
            else
            {
                target.position = LimbConstructor.Extremity.position;
            }
        }
        #endregion
    }
}