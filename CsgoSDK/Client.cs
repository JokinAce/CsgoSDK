using CsgoSDK.Data;
using CsgoSDK.Entities;
using Process.NET;
using Process.NET.Memory;

namespace CsgoSDK {

    public class Client {
        private static Client? Instance { get; set; }
        public ProcessSharp Process { get; private set; }
        private IntPtr ClientDLL { get; set; }

        private ExternalProcessMemory processMemory;

        private Client(ref ProcessSharp processSharp, ref ExternalProcessMemory externalProcessMemory) {
            this.Process = processSharp;
            this.processMemory = externalProcessMemory;
            this.ClientDLL = this.Process["client.dll"].BaseAddress;
        }

        public static Client GetInstance(ref ProcessSharp processSharp, ref ExternalProcessMemory externalProcessMemory) {
            if (Instance == null) {
                Instance = new Client(ref processSharp, ref externalProcessMemory);
            }

            return Instance;
        }

        public CBasePlayer GetLocalPlayer() {
            IntPtr entityAddress = this.processMemory.Read<IntPtr>(this.ClientDLL + Offsets.dwLocalPlayer);
            Entity entity = new(entityAddress, ref this.processMemory);
            return new CBasePlayer(entity);
        }

        public Entity GetEntity(int entityID) {
            IntPtr entity = this.processMemory.Read<IntPtr>(this.ClientDLL + Offsets.dwEntityList + (entityID * 0x10));

            return new Entity(entity, ref this.processMemory);
        }

        public CBasePlayer GetCrosshairTarget() {
            CBasePlayer localPlayer = this.GetLocalPlayer();
            Entity entity = this.GetEntity(localPlayer.CrosshairID - 1);
            return new(entity);
        }

        public bool IsBombPlanted() {
            IntPtr gameRulesProxy = this.processMemory.Read<IntPtr>(this.ClientDLL + Offsets.dwGameRulesProxy);
            return this.processMemory.Read<bool>(gameRulesProxy + Offsets.m_bBombPlanted);
        }

        public void Attack() {
            this.processMemory.Write(this.ClientDLL + Offsets.dwForceAttack, 6);
        }

        public void Right() {
            this.processMemory.Write(this.ClientDLL + Offsets.dwForceRight, 6);
        }

        public void Left() {
            this.processMemory.Write(this.ClientDLL + Offsets.dwForceLeft, 6);
        }

        public void Jump() {
            this.processMemory.Write(this.ClientDLL + Offsets.dwForceJump, 6);
        }
    }
}