public class BoostrapConfig {
    public static BoostrapConfig Instance { get; private set; }

    public static void Use(BoostrapConfig config) {
        Instance = config;
    }
    
    public bool IsStartClient;
    public bool IsStartServer;
}