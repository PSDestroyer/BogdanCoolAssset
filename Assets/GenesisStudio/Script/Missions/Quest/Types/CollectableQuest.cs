using GenesisStudio;
using UnityEngine;

[CreateAssetMenu(fileName = "Collect", menuName = "Genesis Studio/Mission/Collectable Quest")]
public class CollectableQuest : Quest
{
    public ItemData ItemToCollect { get; private set; }
    public int CollectAmount { get; private set; }
    int collected;
    
    public void OnItemCollected(ICharacter character,ItemData item)
    {
        if(item == ItemToCollect)
        {
            collected++;
            AppendToTask($"{collected}/{CollectAmount}");
            if (collected >= CollectAmount)
                Complete();
        }
    }

    public override void OnInitialize(QuestParams @params)
    {
        ItemToCollect = @params.Target_item;
        CollectAmount = @params.Amount;

        if (CollectAmount == 0)
            throw new System.NullReferenceException($"Quest {@params.Task} has collect amount {@params.Amount}");

        if (string.IsNullOrEmpty(_task))
            _task = $"Collect {CollectAmount} {ItemToCollect.ItemName}";

        AppendToTask($"{collected}/{CollectAmount}");
        GameEventBus.Instance.OnItemAdded += OnItemCollected;
    }

    public override void OnComplete()
    {
        collected = 0;
        CollectAmount = 0;
        ItemToCollect = null;
        GameEventBus.Instance.OnItemAdded -= OnItemCollected;
    }

    public override bool IsAlreadyCompleted()
    {
        var item = _player.Inventory().GetItemByData(ItemToCollect);
        return item?.Count >= CollectAmount;
    }
}
