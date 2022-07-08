using UnityEngine;

namespace Levitan {
    public class IDraggable : MonoBehaviour {
        public void DestroyDraggable() {
            Destroy(gameObject);
        }
    }
}