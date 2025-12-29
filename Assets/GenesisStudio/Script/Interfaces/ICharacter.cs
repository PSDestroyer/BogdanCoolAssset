using GenesisStudio;
using System;

public interface ICharacter
{
    void Controls(bool value);
    Inventory Inventory();
    bool GiveItem(ICharacter to, ItemData data);
    void Initialize();

}