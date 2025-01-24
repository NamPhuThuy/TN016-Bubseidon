using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NamPhuThuy
{
    public class SimpleSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();
                    if (objs.Length > 0)
                    {
                        T instance = objs[0];
                        _instance = instance;
                    }
                    else
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        _instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }

                }

                return _instance;
            }
        }
        
        protected void Awake()
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<T>();
                OnInitialization();
            }
        }
        
        public virtual void OnDestroy()
        {
            _instance = null;
            OnExtinction();
        }
        
        public virtual void OnInitialization() { }
        public virtual void OnExtinction() { }
    }
}