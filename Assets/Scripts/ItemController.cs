using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    private const int MAX_ITEM_COUNT = 4;

    public Dictionary<ItemID, Button> itemMenuButtons;
    public Button buttonTop;
    public Button buttonRight;
    public Button buttonBottom;
    public Button buttonLeft;
    public Dictionary<ItemID, GameObject[]> items;
    public GameObject[] itemTop;
    public GameObject[] itemRight;
    public GameObject[] itemBottom;
    public GameObject[] itemLeft;
    public Dictionary<ItemID, bool> unlockedItems;
    public bool itemTopUnlocked;
    public bool itemRightUnlocked;
    public bool itemBottomUnlocked;
    public bool itemLeftUnlocked;

    private ItemID equipedItem;
    public ItemID EquipedItem
    {
        get
        {
            return equipedItem;
        }
    }
    private ItemID highlightedItem;
    public ItemID HighlightedItem
    {
        get
        {
            return highlightedItem;
        }
    }

    private void Start()
    {
        itemMenuButtons = new Dictionary<ItemID, Button>
        {
            { ItemID.None, null },
            { ItemID.Top, buttonTop },
            { ItemID.Right, buttonRight },
            { ItemID.Bottom, buttonBottom },
            { ItemID.Left, buttonLeft }
        };
        unlockedItems = new Dictionary<ItemID, bool>
        {
            { ItemID.None, true },
            { ItemID.Top, itemTopUnlocked },
            { ItemID.Right, itemRightUnlocked },
            { ItemID.Bottom, itemBottomUnlocked },
            { ItemID.Left, itemLeftUnlocked }
        };
        items = new Dictionary<ItemID, GameObject[]>
        {
            { ItemID.None, null },
            { ItemID.Top, itemTop },
            { ItemID.Right, itemRight },
            { ItemID.Bottom, itemBottom },
            { ItemID.Left, itemLeft }
        };

        highlightedItem = ItemID.None;
        equipedItem = ItemID.None;
        UpdateItemMenu();
    }

    public void HighlightItemButton(ItemID id)
    {
        if (unlockedItems[id])
        {
            highlightedItem = id;
        }
        else
        {
            highlightedItem = ItemID.None;
        }

        if (highlightedItem == ItemID.None)
        {
            GUIController.Instance.EventSystem.SetSelectedGameObject(null);
        }
        else
        {
            GUIController.Instance.EventSystem.SetSelectedGameObject(itemMenuButtons[highlightedItem].gameObject);
        }
    }

    private void UpdateItemMenu()
    {
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            if (id == ItemID.None)
            {
                continue;
            }
            if (unlockedItems[id])
            {
                // enable button 
                itemMenuButtons[id].interactable = true;
                if (equipedItem == id)
                {
                    ShowItem(id);
                }
                else
                {
                    HideItem(id);
                }
                // TODO FIXME setting alpha / transparency to half does not work
                //Image img = itemMenuButtons[id].GetComponentInChildren<Image>();
                //Color c = img.color;
                //c.a = 1.0f;
                //img.color = c;
            }
            else
            {
                // disable button
                itemMenuButtons[id].interactable = false;
                HideItem(id);
                // TODO FIXME setting alpha / transparency to half does not work
                //Image img = itemMenuButtons[id].GetComponentInChildren<Image>();
                //Color c = img.color;
                //c.a = 0.5f;
                //img.color = c;
            }
        }
    }

    public void SwitchItem()
    {
        equipedItem = highlightedItem;
        HideAllItems();
        if (equipedItem == ItemID.None)
        {
            GUIController.Instance.EventSystem.firstSelectedGameObject = null;
        }
        else
        {
            ShowItem(equipedItem);
            GUIController.Instance.EventSystem.firstSelectedGameObject = itemMenuButtons[equipedItem].gameObject;
        }
    }

    private void ShowAllItems()
    {
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            if (id == ItemID.None)
            {
                continue;
            }
            ShowItem(id);
        }
    }

    private void HideAllItems()
    {
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            if (id == ItemID.None)
            {
                continue;
            }
            HideItem(id);
        }
    }

    private void ShowItem(ItemID id)
    {
        if (id == ItemID.None)
        {
            HideAllItems();
        }
        else
        {
            SetActiveItem(id, true);
        }
    }

    private void HideItem(ItemID id)
    {
        if (id == ItemID.None)
        {
            HideAllItems();
        }
        else
        {
            SetActiveItem(id, false);
        }
    }

    private void SetActiveItem(ItemID id, bool show)
    {
        foreach (GameObject obj in items[id])
        {
            if (obj != null)
            {
                obj.SetActive(show);
            }
        }
    }

    private void ChangeLockOnItem(ItemID id, bool unlocked)
    {
        if (id != ItemID.None)
        {
            unlockedItems[id] = unlocked;
            UpdateItemMenu();
        }
    }

    public void LockItem(ItemID id)
    {
        ChangeLockOnItem(id, false);
    }

    public void UnlockItem(ItemID id)
    {
        ChangeLockOnItem(id, true);
    }

    private void ChangeLockOnAllItems(bool unlocked)
    {
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            if (unlocked)
            {
                UnlockItem(id);
            }
            else
            {
                LockItem(id);
            }
        }
    }

    public void LockAllItems()
    {
        ChangeLockOnAllItems(false);
    }

    public void UnlockAllItems()
    {
        ChangeLockOnAllItems(true);
    }
}

public enum ItemID
{
    None, Top, Right, Bottom, Left
}
