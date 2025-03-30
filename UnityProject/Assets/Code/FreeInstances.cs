using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code {
    public class FreeInstances : MonoBehaviour {
        private static FreeInstances _instances;
        public static FreeInstances Instance {
            get {
                if (_instances == null) {
                    _instances = new GameObject("FreeInstances").AddComponent<FreeInstances>();
                    DontDestroyOnLoad(_instances.gameObject);
                }

                return _instances;
            }
        }
        private Dictionary<Type, IFreeInstance> instancesManaged = new Dictionary<Type, IFreeInstance>();

        public void Add<T>(IFreeInstance instance) {
            var type = typeof(T);
            instancesManaged.Add(type, instance);
        }
    }
    
    public interface IFreeInstance { }

    public abstract class FreeInstance : MonoBehaviour, IFreeInstance { }
}
