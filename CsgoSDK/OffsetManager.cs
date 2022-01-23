namespace CsgoSDK {

    internal class OffsetManager : IDisposable {
        private static OffsetManager? Instance { get; set; }

        private OffsetManager() {
        }

        public static OffsetManager GetInstance() {
            if (Instance == null) {
                Instance = new OffsetManager();
            }

            return Instance;
        }

        public void Dispose() {
            Instance = null;
        }
    }
}