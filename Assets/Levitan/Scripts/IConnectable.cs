using System.Collections;
using System.Collections.Generic;
using Levitan;
using UnityEngine;

namespace Levitan {
    public interface IConnectable {

        public string ID { get; }
        
        public string Name { get; }

        public Vector3 Position { get; }

        public void SpawnConnection() {
            Connection connection = AppManager.instance._workspaceManager.InstantiateConnection(this);
            connection.StartDrag();
            CameraController.IsDrawingLine = true;
        }

        public bool CanAddConnection(IConnectable start);

        public void AddConnection(Connection connection);

        public Vector3 GetRectEdgeForPosition(Vector3 position);

        public void RemoveConnection(Connection connection);

        public bool HasSameConnection(string target);
    }
}