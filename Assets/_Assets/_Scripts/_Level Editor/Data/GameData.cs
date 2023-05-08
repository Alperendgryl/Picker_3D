using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<ObjectData> levelChildren;

    [System.Serializable]
    public class ObjectData
    {
        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;

        public ObjectData(int prefabIndex, Vector3 position, Quaternion rotation)
        {
            this.prefabIndex = prefabIndex;
            this.position = position;
            this.rotation = rotation;
        }
    }
}
