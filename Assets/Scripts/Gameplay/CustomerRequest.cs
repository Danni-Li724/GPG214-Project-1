using Game.Gameplay;

namespace Game.Gameplay
{
    public struct CustomerRequest
    {
        public int CargoTypeId;
        public PackagingType PackagingType;

        public CustomerRequest(int cargoTypeId, PackagingType packagingType)
        {
            CargoTypeId = cargoTypeId;
            PackagingType = packagingType;
        }
    }
}
