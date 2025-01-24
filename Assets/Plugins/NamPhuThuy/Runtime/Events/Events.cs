using UnityEngine;
using UnityEngine.Events;

namespace NamPhuThuy
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
}