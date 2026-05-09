using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventoryPhoneAppUI : InventorySystem
{
    private List<PhoneUIScreen> _appScreens = new List<PhoneUIScreen>();

    public bool IsInventoryOpen => _appScreens.Count > 0;

    public virtual void AddInventoryScreen(PhoneUIScreen newScreen, PlayerManager playerManager)
    {
        _appScreens.Add(newScreen);

        if (_appScreens.Count == 1)
        {
            playerManager.UIManager.DragAndDropItems.DragAndDropData.Add(new DragAndDropData(this));
        }

        DragAndDropData dragAndDropData = GetNecessaryDragAndDropData(playerManager);

        dragAndDropData.ItemsGrids.Add(new List<List<VisualElement>>());
        dragAndDropData.ItemPlacesGrids.Add(new List<List<VisualElement>>());

        InventoryRender(playerManager);
    }

    public virtual void RemoveInventoryScreen(PhoneUIScreen removeScreen, PlayerManager playerManager)
    {
        int removeScreenIndex = _appScreens.IndexOf(removeScreen);

        if (removeScreenIndex == -1) { return; }

        _appScreens.RemoveAt(removeScreenIndex);

        DragAndDropData dragAndDropData = GetNecessaryDragAndDropData(playerManager);

        dragAndDropData.ItemsGrids.RemoveAt(removeScreenIndex);
        dragAndDropData.ItemPlacesGrids.RemoveAt(removeScreenIndex);

        if (IsInventoryOpen == false)
        {
            playerManager.UIManager.DragAndDropItems.DragAndDropData.Remove(dragAndDropData);
        }
    }

    public void CalculateDragAndDropGrids(PlayerManager playerManager, bool isInventoryRender = false)
    {
        DragAndDropData necessaryDragAndDropData = GetNecessaryDragAndDropData(playerManager);

        if (necessaryDragAndDropData == null) { return; }

        Vector2Int maxAxis = InventoryGridMaxAxis;

        foreach (List<List<VisualElement>> itemsGrid in necessaryDragAndDropData.ItemsGrids)
        {
            CalculateGrid(itemsGrid, maxAxis);
        }

        foreach (List<List<VisualElement>> itemPlacesGrid in necessaryDragAndDropData.ItemPlacesGrids)
        {
            CalculateGrid(itemPlacesGrid, maxAxis);
        }

        for (int appIndex = 0; appIndex < _appScreens.Count; appIndex++)
        {
            VisualElement root = _appScreens[appIndex].AppUIDocument.rootVisualElement;
            VisualElement contentContainer = root.Q<VisualElement>(className: ContentContainerClassName);

            VisualElement[] slots = GetContents(contentContainer, ContentSlot.ContentClassName);
            VisualElement[] items = GetContents(contentContainer, ContentItem.ContentClassName);

            Array.Reverse(slots);
            Array.Reverse(items);

            int slotIndex = 0;
            int itemIndex = 0;

            for (int x = 0; x < maxAxis.x; x++)
            {
                for (int y = 0; y < maxAxis.y; y++)
                {
                    necessaryDragAndDropData.ItemPlacesGrids[appIndex][x][y] = slots[slotIndex];
                    slotIndex++;

                    if (InventoryGrid[x][y] == null)
                    {
                        necessaryDragAndDropData.ItemsGrids[appIndex][x][y] = null;
                    }
                    else
                    {
                        necessaryDragAndDropData.ItemsGrids[appIndex][x][y] = items[itemIndex];
                        itemIndex++;
                    }
                }
            }
        }

        if (isInventoryRender == true) { InventoryRender(playerManager); }
    }

    public override void InventoryRender(PlayerManager playerManager)
    {
        foreach (PhoneUIScreen appScreen in _appScreens)
        {
            RenderAppScreen(appScreen, playerManager);
        }

        CalculateDragAndDropGrids(playerManager);
    }

    public virtual void RenderAppScreen(PhoneUIScreen appScreen, PlayerManager playerManager)
    {
        RenderInventoryUI(appScreen.AppUIDocument, isFixedY: false);
    }
}
