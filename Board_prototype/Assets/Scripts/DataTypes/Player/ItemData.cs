

public class ItemData
{
    public string itemName;
    public System.UInt64 baseCoast;
    public System.UInt64 baseGrowthRate;
    public int level;

    public bool isBought;

    //TODO:здесь добавить ссылку на изображение или я хуй знает чето придумать вообщем

    public ItemData(string _itemName, System.UInt64 _baseCoast, System.UInt64 _baseGrowthRate, int _level, bool _isBought)
    {
        itemName = _itemName;
        baseCoast = _baseCoast;
        baseGrowthRate = _baseGrowthRate;
        level = _level;
        isBought = _isBought;
    }
    public ItemData() { }
}

