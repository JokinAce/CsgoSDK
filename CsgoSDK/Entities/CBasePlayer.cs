using CsgoSDK.Data;

namespace CsgoSDK.Entities {

    public struct CBasePlayer : IEntity {
        public Entity Entity { get; private set; }

        public IntPtr Address { get; private set; }

        public CBasePlayer(Entity entity) {
            this.Entity = entity;
            this.Address = entity.Address;
        }

        public bool Spotted {
            set => this.Entity.Write(Offsets.m_bSpotted, value);
        }

        public int CrosshairID => this.Entity.Read<int>(Offsets.m_iCrosshairId);

        public bool Dormant => this.Entity.Read<bool>(Offsets.m_bDormant);

        public int Flags => this.Entity.Read<int>(Offsets.m_fFlags);

        public int Health => this.Entity.Read<int>(Offsets.m_iHealth);

        public int ImmuneFlag => this.Entity.Read<int>(Offsets.m_bGunGameImmunity);

        public int Team => this.Entity.Read<int>(Offsets.m_iTeamNum);

        public bool IsDefusing => this.Entity.Read<bool>(Offsets.m_bIsDefusing);

        public bool HasDefuser => this.Entity.Read<bool>(Offsets.m_bHasDefuser);

        public bool IsValid() {
            return this.Entity.Address != default && !this.Dormant && this.ImmuneFlag == 256 && this.Health > 0;
        }
    }
}