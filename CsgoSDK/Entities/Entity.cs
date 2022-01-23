using CsgoSDK.Data;
using Process.NET.Memory;
using System.ComponentModel;

namespace CsgoSDK.Entities {

    public interface IEntity {
        public Entity Entity { get; }

        public IntPtr Address { get; }

        public bool IsValid();
    }

    public class Entity {
        public IntPtr Address { get; set; }

        private ExternalProcessMemory Client { get; set; }

        public Entity(IntPtr address, ref ExternalProcessMemory client) {
            this.Address = address;
            this.Client = client;
        }

        public T Read<T>(int offset) {
            return this.Client.Read<T>(this.Address + offset);
        }

        public void Write<T>(int offset, T value) {
            this.Client.Write<T>(this.Address + offset, value);
        }

        public ClassID GetClassID() {
            try {
                IntPtr vt = this.Client.Read<IntPtr>(this.Address + 0x8);
                IntPtr fn = this.Client.Read<IntPtr>(vt + 2 * 0x4);
                IntPtr cls = this.Client.Read<IntPtr>(fn + 0x1);
                return (ClassID)this.Client.Read<int>(cls + 0x14);
            } catch (Win32Exception) {
                return 0;
            }
        }
    }
}