using System.Collections.Generic;
using Levitan;
using UnityEngine;

public class TransitionsHolder : MonoBehaviour {
    [SerializeField]
    public DraggableDialog _dialog;

    [SerializeField]
    private Transition TransitionPrefab;

    [SerializeField]
    private Transform PlusButton;

    private List<Transition> transitions;

    public void Init() {
        transitions = new List<Transition>();
    }

    public void InstantiateTransition() {
        DraggableData data = new() {
            _connectionsList = new List<ConnectionData>(),
            _dialogData = new DialogData() {
                ID = _dialog.ID
            },
            ID = System.Guid.NewGuid().ToString()
        };
        AppManager.instance._workspaceManager.InstantiateTransition(data);
    }

    public List<TransitionData> CollectTransitionsData() {
        List<TransitionData> datas = new();
        foreach (var transition in transitions) {
            try {
                TransitionData data = new() {
                    dialog = transition.DialogConnected.ID,
                    thought = transition.ThoughtConnected.Name
                };
                datas.Add(data);
            }
            catch {
                Debug.Log("Transition " + transition.name + " is incorrect.");
            }
        }

        return datas;
    }

    public void RedrawConnections() {
        foreach (var transition in transitions) {
            transition.RedrawConnections();
        }
    }

    public void AddEmptyTransition(IDraggable draggable) {
        draggable.transform.SetParent(transform);
        Transition transition = draggable as Transition;
        transition.transform.localPosition = new Vector3(0, transitions.Count * -1, 0);
        transition.OnDestroyPressed += RemoveTransition;
        transition.Init(transitions.Count);
        transitions.Add(transition);
        PlusButton.localPosition = new Vector3(0, transitions.Count * -1, 0);
    }

    public void RemoveTransition(int number) {
        Transition transition = transitions[number];
        transitions.RemoveAt(number);
        PlusButton.localPosition = new Vector3(0, transitions.Count * -1, 0);
        for (int i = 0; i < transitions.Count; i++) {
            transitions[i].transform.localPosition = new Vector3(0, i * -1, 0);
            transitions[i]._number = i;
        }
        RedrawConnections();
    }
}