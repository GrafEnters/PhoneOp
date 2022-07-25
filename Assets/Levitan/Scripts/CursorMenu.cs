using Levitan;
using UnityEngine;

public class CursorMenu : MonoBehaviour {
    public void CreateTag() {
        AppManager.instance._workspaceManager.InstantiateTag();
        gameObject.SetActive(false);
    }

    public void CreateDialog() {
        AppManager.instance._workspaceManager.InstantiateDialog();
        gameObject.SetActive(false);
    }
    
    public void CreateThought() {
        AppManager.instance._workspaceManager.InstantiateThought();
        gameObject.SetActive(false);
    }
}