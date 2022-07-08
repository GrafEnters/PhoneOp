using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Levitan {
    public class Background : MonoBehaviour, IPointerDownHandler {
        [SerializeField]
        private UnityEvent OnDragAction;

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == 0) {
                OnDragAction?.Invoke();
            }
        }
    }
}