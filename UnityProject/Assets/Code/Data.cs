namespace Code {

    public interface IFeature {
        void OnCreate() { }
        void OnUpdate(ref FeatureState state) { }
        void OnDestroy() { }
    }

    public interface IEnableFeature : IFeature {
        bool IsEnabled();
    }

    public struct FeatureState {
        public FeatureManager FeatureMgr;
    }
}