
using System.Collections.Generic;

public interface IWinCondition
{
    public Dictionary<byte, bool> playerList { get; set; } //int => playerID, bool => hasWon;
    public bool HasWon(byte id) => WinCondition(id) ? playerList[id] = true : false;
    public void AddPlayer(byte id) => playerList[id] = false;
    public bool WinCondition(byte id);
}
