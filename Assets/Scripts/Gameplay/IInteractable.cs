namespace Game.Gameplay
{
    public interface IInteractable
    {
        bool CanInteract(PlayerInteract player);
        void Interact(PlayerInteract player);
    }
}