using Process.NET;
using Process.NET.Memory;

namespace CsgoSDK {

    public class CsgoService {
        private static CsgoService? Instance { get; set; }
        public Overlay Overlay { get; private set; }
        public Client Client { get; private set; }
        public Engine Engine { get; private set; }

        public readonly ProcessSharp process;
        private readonly ExternalProcessMemory processMemory;

        private CsgoService() {
            this.process = new("csgo", MemoryType.Remote);
            this.processMemory = new(this.process.Handle);

            this.Client = new(ref this.processMemory, this.process["client.dll"].BaseAddress);
            this.Engine = new(ref this.processMemory, this.process["engine.dll"].BaseAddress);
            this.Overlay = new(this.process.WindowFactory.MainWindow.Width, this.process.WindowFactory.MainWindow.Height, this.process.Native.Handle);
        }

        public static CsgoService GetInstance() {
            if (Instance == null) {
                Instance = new CsgoService();
            }

            return Instance;
        }

        public bool IsRunning => !this.process.Native.HasExited;
    }
}