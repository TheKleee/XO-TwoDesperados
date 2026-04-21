using System.Collections.Generic;
using UnityEngine;

public class SkinPicker : MonoBehaviour
{
    [SerializeField] Sprite[] skins; // same 6 skins for preview in UI
    [SerializeField] byte playerCount;

    List<byte> availableSkins; // pool shrinks as players pick
    byte currentPickingPlayer = 1;

    private void Start()
    {
        availableSkins = new List<byte>();
        for (byte i = 0; i < skins.Length; i++)
            availableSkins.Add(i);

        ShowPickerForPlayer(currentPickingPlayer);
    }

    // Called by UI buttons, passing the skinId of the button pressed
    public void OnSkinSelected(byte skinId)
    {
        if (!availableSkins.Contains(skinId)) return;

        availableSkins.Remove(skinId);
        currentPickingPlayer++;

        if (currentPickingPlayer > playerCount)
            OnAllPlayersPicked();
        else
            ShowPickerForPlayer(currentPickingPlayer);
    }

    void ShowPickerForPlayer(byte playerId)
    {
        Debug.Log($"Player {playerId} pick a skin");
        // TODO: refresh UI, grey out unavailable skins
    }

    void OnAllPlayersPicked()
    {
        Debug.Log("All players have picked, ready to start");
        // TODO: transition to game scene
    }

}