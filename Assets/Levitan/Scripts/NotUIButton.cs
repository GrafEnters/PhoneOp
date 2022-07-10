using UnityEngine;
using UnityEngine.Events;

namespace Levitan {
    [RequireComponent(typeof(BoxCollider2D))]
    public class NotUIButton : MonoBehaviour {
        [SerializeField]
        private UnityEvent OnClicked;

        private void OnMouseUpAsButton() {
            OnClicked?.Invoke();
        }
    }
}