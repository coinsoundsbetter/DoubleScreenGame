namespace Code.Client.Player {
    public class Player {
        public int GameId { get; set; }
        public virtual bool IsLocalPlayer => false;
        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void Destroy() { }
    }
}