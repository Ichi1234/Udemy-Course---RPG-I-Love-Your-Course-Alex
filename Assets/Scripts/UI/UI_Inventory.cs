using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private UI_EquipSlotParent uiEquipSlotParent;


    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
    }

}
