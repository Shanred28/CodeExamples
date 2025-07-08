using GamePlayLogic.GameService.InteractionItemModule.InventorySystem;

namespace GamePlayLogic.GameService.InteractionItemModule
{
    public interface IInventory
    {
        InventoryService InventoryService { get; }
    }
}