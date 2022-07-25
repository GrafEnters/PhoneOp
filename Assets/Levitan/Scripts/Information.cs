using System;
using System.Collections;
using System.Collections.Generic;
using Levitan;
using TMPro;
using UnityEngine;

public class Information : IDraggable {

    [SerializeField]
    private GameObject plusButton;

    [SerializeField]
    private TextMeshProUGUI lineText;

    
    public IConnectable ThoughtConnected;
    public int _lineIndex;
    public void Init(int lineIndex, string line) {
      
        _connections = new List<Connection>();
        _lineIndex = lineIndex;
        lineText.text = $@"<i><color=grey>{lineIndex}.</color></i> {line}";
        gameObject.SetActive(true);
    }

    public void Destroy() {
        DestroyDraggable();
    }

    public override void ChangeDialogName(string newName) {
    }

    public override bool CanAddConnection(IConnectable start) {
        return false;
    }

    public override Vector3 GetRectEdgeForPosition(Vector3 position) {
        Vector3 res = transform.position + Vector3.right * (2.5f * Mathf.Sign(position.x - transform.position.x));
        res.z = 0;
        return res;
    }

    protected override void OnMouseDrag() {
    }

    public override void AddConnection(Connection connection) {
        base.AddConnection(connection);
        if ((connection._endPoint as IDraggable)._data.Type == DraggableType.Thought) {
            ThoughtConnected = connection._endPoint;
        }

        plusButton.SetActive(false);
    }

    public override void RemoveConnection(Connection connection) {
        base.RemoveConnection(connection);
        ThoughtConnected = null;
        plusButton.SetActive(true);
    }
}