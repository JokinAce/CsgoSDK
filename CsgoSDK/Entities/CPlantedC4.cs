using CsgoSDK.Data;

namespace CsgoSDK.Entities {

    public struct CPlantedC4 : IEntity {
        public Entity Entity { get; private set; }
        public IntPtr Address { get; private set; }

        public CPlantedC4(Entity entity) {
            this.Entity = entity;
            this.Address = entity.Address;
        }

        public bool HasDefuser => this.Entity.Read<uint>(Offsets.m_hBombDefuser) != 0xFFFFFFFF;

        public float DefuseLength => this.Entity.Read<float>(Offsets.m_flDefuseLength);

        public bool IsValid() {
            ClassID classID = this.Entity.GetClassID();
            return classID == ClassID.CPlantedC4;
        }
    }
}