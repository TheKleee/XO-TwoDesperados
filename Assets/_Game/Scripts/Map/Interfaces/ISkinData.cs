using System.Collections.Generic;

public interface ISkinData
{
    Dictionary<byte, byte> playerSkinMap { get; set; } // playerId -> skinId
    public void ClearSkins() => playerSkinMap.Clear();
}