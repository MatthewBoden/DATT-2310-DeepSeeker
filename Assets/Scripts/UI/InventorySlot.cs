using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColour, notSelectedColour;

    private void Awake()
    {
        Deselect();
    }
    public void Select() {
        image.color = selectedColour;
    }

    public void Deselect()
    {
        image.color = notSelectedColour;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) { 
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
