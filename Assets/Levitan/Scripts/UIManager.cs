using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levitan {
    public class UIManager : MonoBehaviour,IAppModule {
        [SerializeField]
        private RectTransform _cursorMenu;



        public void ShowCursorMenu(Vector3 position) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                Input.mousePosition, Camera.main, 
                out Vector2 movePos);

            //transform.position = transform.TransformPoint(movePos);
            
            
            _cursorMenu.gameObject.SetActive(true);
            //position.z =   _cursorMenu.localPosition.z;
            _cursorMenu.position =  transform.TransformPoint(movePos);
        }

        public void Init() {
          
        }
    } 
}


