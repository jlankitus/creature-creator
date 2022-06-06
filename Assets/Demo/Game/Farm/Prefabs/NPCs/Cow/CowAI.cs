// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public class CowAI : AnimalAI
    {
        #region Fields
        [SerializeField] protected TrackRegion chargeRegion;
        #endregion

        #region Methods
        public override void Start()
        {
            base.Start();

            chargeRegion.OnTrack += delegate
            {
                ChangeState("CHA");
            };
            chargeRegion.OnLoseTrackOf += delegate
            {
                if (chargeRegion.tracked.Count == 0)
                {
                    ChangeState("WAN");
                }
            };
        }
        #endregion

        #region Inner Classes
        [Serializable]
        public class Charging : BaseState
        {
            [SerializeField] private float chargeSpeedMultiplier;
            [SerializeField] private float chargeUpTime;
            [SerializeField] private string[] chargeNoises;
            [Space]
            [SerializeField] private float restTime;
            [SerializeField] private string[] restNoises;
            [Space]
            [SerializeField] private float hornDistance;
            [SerializeField] private Vector3 chargeForce;
            [SerializeField] private MinMax chargeDamage;
            [SerializeField] private string[] impactNoises;

            private CowAI CowAI => StateMachine as CowAI;

            public Charging(CowAI cowAI) : base(cowAI) { }

            public override void Enter()
            {
                CowAI.agent.speed *= chargeSpeedMultiplier;

                Charge(CowAI.chargeRegion.Nearest.transform);
            }
            public override void Exit()
            {
                CowAI.agent.speed /= chargeSpeedMultiplier;
            }

            private void Charge(Transform charged)
            {
                CowAI.StartCoroutine(ChargeRoutine(charged));
            }
            private IEnumerator ChargeRoutine(Transform charged)
            {
                // Rotate to charged.
                Quaternion cur = CowAI.transform.rotation;
                Quaternion tar = Quaternion.LookRotation(Vector3.ProjectOnPlane(charged.position - CowAI.transform.position, CowAI.transform.up));
                CowAI.agent.updateRotation = false;
                yield return InvokeUtility.InvokeOverTimeRoutine(delegate (float progress)
                {
                    CowAI.transform.rotation = Quaternion.Slerp(cur, tar, progress);
                }, 1f);
                CowAI.agent.updateRotation = true;

                // Charge!
                CowAI.creature.Effector.PlaySound(chargeNoises);
                yield return new WaitForSeconds(chargeUpTime);
                CowAI.agent.SetDestination(charged.position);
                List<Collider> hit = new List<Collider>();
                while (CowAI.IsMovingToPosition)
                {
                    Vector3 checkPos = CowAI.transform.position + CowAI.transform.forward * hornDistance;

                    Collider[] colliders = Physics.OverlapSphere(checkPos, 0.5f);
                    foreach (Collider collider in colliders)
                    {
                        if (hit.Contains(collider)) continue;
                        hit.Add(collider);

                        CreatureBase creature = collider.GetComponent<CreatureBase>();
                        if (creature != null && creature != CowAI.creature)
                        {
                            Vector3 dir = (creature.transform.position - CowAI.transform.position).normalized;
                            Vector3 force = dir * chargeForce.z + creature.transform.up * chargeForce.y;

                            creature.Health.TakeDamage(chargeDamage.Random);
                            if (!creature.Health.IsDead)
                            {
                                creature.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                            }
                            CowAI.creature.Effector.PlaySound(impactNoises);
                        }
                    }

                    yield return null;
                }

                // Rest...
                CowAI.creature.Effector.PlaySound(restNoises);
                yield return new WaitForSeconds(restTime);

                // Charge again?
                if (CowAI.chargeRegion.tracked.Count != 0)
                {
                    Charge(CowAI.chargeRegion.Nearest.transform);
                }
                else
                {
                    CowAI.ChangeState("WAN");
                }
            }
        }
        #endregion
    }
}