namespace Code {

    public interface IFeature {
        void OnCreate() { }
        void OnUpdate(ref FeatureState state) { }
        void OnDestroy() { }
    }

    public interface ILateUpdateFeature : IFeature {
        void OnLateUpdate(ref FeatureState state) { }
    }

    public interface IEnableFeature : IFeature {
        bool IsEnabled();
    }

    public struct FeatureState {
        public FeatureManager FeatureMgr;
    }
}