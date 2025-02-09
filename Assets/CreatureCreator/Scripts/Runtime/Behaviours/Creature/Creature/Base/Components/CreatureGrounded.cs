using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureCollider))]
    [RequireComponent(typeof(CreatureAnimator))]
    public class CreatureGrounded : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float contactDistance;
        #endregion

        #region Properties
        public float ContactDistance => contactDistance;

        public CreatureCollider Collider { get; private set; }
        public CreatureAnimator Animator { get; private set; }

        public bool IsGrounded
        {
            get => Animator.Animator.GetBool("IsGrounded");
            set => Animator.Animator.SetBool("IsGrounded", value);
        }
        #endregion

        #region Methods
        private void Awake()
        {
            Collider = GetComponent<CreatureCollider>();
            Animator = GetComponent<CreatureAnimator>();
        }
        private void OnDisable()
        {
            IsGrounded = false;
        }
        private void FixedUpdate()
        {
            HandleGrounded();
        }

        private void HandleGrounded()
        {
            bool isGrounded = Physics.Raycast(transform.position + transform.up * contactDistance, -transform.up, 2f * contactDistance);
            if (!isGrounded)
            {
                foreach (LegAnimator leg in Animator.Legs)
                {
                    if (leg != null)
                    {
                        if (CheckLeg(leg) || CheckLeg(leg.FlippedLeg))
                        {
                            isGrounded = true;
                            break;
                        }
                    }
                }
            }
            IsGrounded = isGrounded;
        }

        private bool CheckLeg(LegAnimator leg)
        {
            return PhysicsUtility.RaycastCone(leg.transform.position, -transform.up, leg.MaxLength, 15f, 3, 10, LayerMask.GetMask("Ground"), transform.up, 0.5f, out RaycastHit? hitInfo) != null;
        }
        #endregion
    }
}