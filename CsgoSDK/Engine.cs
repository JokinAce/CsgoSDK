using CsgoSDK.Data;
using Process.NET;
using Process.NET.Memory;
using System.Numerics;

namespace CsgoSDK {

    public class Engine {
        private IntPtr EngineDLL { get; set; }

        private readonly ExternalProcessMemory processMemory;

        public Engine(ref ExternalProcessMemory externalProcessMemory, IntPtr engineDLL) {
            this.processMemory = externalProcessMemory;
            this.EngineDLL = engineDLL;
        }

        public bool SendPackets {
            get => this.processMemory.Read<bool>(this.EngineDLL + Offsets.dwbSendPackets);
            set => this.processMemory.Write(this.EngineDLL + Offsets.dwbSendPackets, value ? (byte)1 : (byte)0);
        }

        public Vector3 ViewAngles() {
            IntPtr EngineClient = this.processMemory.Read<IntPtr>(this.EngineDLL + Offsets.dwClientState);

            //return new Vector3 {
            //    X = this.processMemory.Read<float>(EngineClient + Offsets.dwClientState_ViewAngles),
            //    Y = this.processMemory.Read<float>(EngineClient + Offsets.dwClientState_ViewAngles + 0x4),
            //    Z = this.processMemory.Read<float>(LocalPlayer.Address + Offsets.m_vecViewOffset + 0x8)
            //};

            return this.processMemory.Read<Vector3>(EngineClient + Offsets.dwClientState_ViewAngles);
        }
    }
}