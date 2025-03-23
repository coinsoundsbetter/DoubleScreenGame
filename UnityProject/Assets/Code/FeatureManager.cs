using System;
using System.Collections.Generic;

namespace Code {
    public class FeatureManager {
        private readonly Dictionary<string, IFeature> allFeatures = new();
        private FeatureState shareState;

        public FeatureManager() {
            shareState = new FeatureState() {
                FeatureMgr = this,
            };
        }

        public void Destroy() {
            foreach (var feature in allFeatures.Values) {
                feature.OnDestroy();
            }
        }

        public void Update() {
            foreach (var feature in allFeatures.Values) {
                if (feature is IEnableFeature enableFeature && !enableFeature.IsEnabled()) {
                    continue;
                }
                
                feature.OnUpdate(ref shareState);
            }
        }

        public void LateUpdate() {
            foreach (var feature in allFeatures.Values) {
                if (feature is IEnableFeature enableFeature && !enableFeature.IsEnabled()) {
                    continue;
                }

                if (feature is ILateUpdateFeature lateUpdateFeature) {
                    lateUpdateFeature.OnLateUpdate(ref shareState);
                }
            }
        }

        public void Add(Type type, IFeature feature) {
            allFeatures.Add(type.Name, feature);
        }

        public T Get<T>() where T : IFeature {
            var key = typeof(T).Name;
            if (allFeatures.TryGetValue(key, out var feature)) {
                return (T)feature;
            }

            return default;
        }

        public CreateFeatureBatch CreateBatch() {
            var newBatch = new CreateFeatureBatch(this);
            return newBatch;
        }
    }

    public struct CreateFeatureBatch {
        private FeatureManager mgr;
        private Dictionary<Type, IFeature> allFeatures;

        public CreateFeatureBatch(FeatureManager featureManager) {
            mgr = featureManager;
            allFeatures = new Dictionary<Type, IFeature>();
        }

        public CreateFeatureBatch Add<T>() where T : IFeature, new() {
            allFeatures.Add(typeof(T), new T());
            return this;
        }

        public void OnCreateAll() {
            foreach (var kvp in allFeatures) {
                mgr.Add(kvp.Key, kvp.Value);
            }

            foreach (var feature in allFeatures.Values) {
                feature.OnCreate();
            }
        }
    }
}