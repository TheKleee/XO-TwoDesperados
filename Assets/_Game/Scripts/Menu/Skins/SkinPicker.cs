using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinPicker : MonoBehaviour
{
    [SerializeField] Button[] skinButtons; // sprites set directly on buttons in inspector
    [SerializeField] TMP_Text promptText;

    List<byte> availableSkins;
    byte currentPickingPlayer;
    byte playerCount;
    bool done;

    public void Init(byte count)
    {
        done = false;
        playerCount = count;
        currentPickingPlayer = 1;
        availableSkins = new List<byte>();
        ((ISkinData)MapBuilder.instance).ClearSkins();

        for (byte i = 0; i < skinButtons.Length; i++)
        {
            availableSkins.Add(i);
            skinButtons[i].interactable = true;
        }
        RefreshPrompt();
    }

    public void OnSkinSelected(int skinId)
    {
        if (done || !availableSkins.Contains((byte)skinId)) return;
        MapBuilder.instance.playerSkinMap[currentPickingPlayer] = (byte)skinId;
        availableSkins.Remove((byte)skinId);
        skinButtons[skinId].interactable = false;
        currentPickingPlayer++;

        if (currentPickingPlayer > playerCount)
        {
            done = true;
            promptText.text = "Ready!";
        }
        else
            RefreshPrompt();
    }

    public void AssignDefaults()
    {
        for (byte p = 1; p <= playerCount; p++)
        {
            if (!MapBuilder.instance.playerSkinMap.ContainsKey(p))
            {
                byte skinId = availableSkins[0];
                MapBuilder.instance.playerSkinMap[p] = skinId;
                availableSkins.Remove(skinId);
                skinButtons[skinId].interactable = false;
            }
        }
    }

    void RefreshPrompt() =>
        promptText.text = $"Player {currentPickingPlayer} pick a skin";
}