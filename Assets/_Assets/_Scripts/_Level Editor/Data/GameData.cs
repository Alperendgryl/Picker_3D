using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameObject levelDesign;
    public List<ObjectData> levelDesignChildren;

    [System.Serializable]
    public class ObjectData
    {
        public int prefabIndex;
        public Vector3 position;
        public Quaternion rotation;
    }
}
