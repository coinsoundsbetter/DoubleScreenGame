namespace Code {
    public class BoostrapContext {
        public bool asServer;
        public bool asClient;
        public static BoostrapContext Instance { get; private set; }
        
        private BoostrapContext() { }
        
        public static BoostrapContext Create() {
            Instance = new BoostrapContext();
            return Instance;
        }
    }
}