using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [RequireComponent(typeof(CreatureConstructor))]
    public class CreatureNamer : PlayerNamer
    {
        #region Properties
        public CreatureConstructor Constructor { get; set; }
        #endregion

        #region Methods
        private void Awake()
        {
            Constructor = GetComponent<CreatureConstructor>();
        }

        public override void Setup()
        {
            base.Setup();

            if (nameGO != null)
            {
                nameGO.transform.localPosition = Vector3.up * (Constructor.Dimensions.height + 0.25f);
            }
        }
        #endregion
    }
}