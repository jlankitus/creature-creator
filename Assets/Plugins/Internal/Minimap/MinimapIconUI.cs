using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DanielLochner.Assets
{
    public class MinimapIconUI : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Image iconImg;
        [SerializeField] private Button iconBtn;

        private RectTransform content;
        #endregion

        #region Properties
        public RectTransform RectTransform => transform as RectTransform;
        #endregion

        #region Methods
        private void Awake()
        {
            content = GetComponentInParent<ScrollRect>().content;
        }
        private void Update()
        {
            transform.localScale = new Vector3(1f / content.localScale.x, 1f / content.localScale.y, 1f);
        }
        public void Setup(Sprite icon, UnityAction onClick, bool isButton)
        {
            iconImg.sprite = icon;
            if (isButton)
            {
                iconBtn.onClick.AddListener(onClick);
            }
            else
            {
                iconBtn.interactable = false;
            }
        }
        #endregion
    }
}