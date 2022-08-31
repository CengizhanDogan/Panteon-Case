using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class BuildingPanel : MonoBehaviour
{
    private ObjectPooler pooler;
    [SerializeField] private List<ButtonBehaviour> buttonList = new List<ButtonBehaviour>();

    private RectTransform scrollPanel;
    [SerializeField] private RectTransform verticalScroll;
    [SerializeField] private RectTransform horizontalScroll;
    [SerializeField] private float scrollSpeed;
    private Vector3 deltaPos;
    private Vector3 firstPos;

    private float scrollPosition;
    private float lastScrollPosition;

    private bool IsWideScreen => Screen.width > Screen.height;
    private void Start()
    {
        pooler = ObjectPooler.Instance;

        SetPanel();
        CreateButtons(IsWideScreen);
    }

    private void SetPanel()
    {
        // Checks screen resolution
        if (IsWideScreen)
        {
            verticalScroll.parent.parent.gameObject.SetActive(true);
            lastScrollPosition = verticalScroll.anchoredPosition.y;
            scrollPanel = verticalScroll;
        }
        else
        {
            horizontalScroll.parent.parent.gameObject.SetActive(true);
            lastScrollPosition = horizontalScroll.anchoredPosition.x;
            scrollPanel = horizontalScroll;
        }
    }

    private void CreateButtons(bool isVertical)
    {
        int x = 0;
        for (int i = 0; i < 24; i++)
        {
            Vector2 pos = SpawnPos(x, isVertical);

            if (i % 2 != 0)
            {
                // Creates button in 2 rows
                if (isVertical) pos *= Vector2.left + Vector2.up;
                else pos *= Vector2.right + Vector2.down;
                //
                x += 150;
            }

            SpawnButton(pos);
        }

        AssignButtons();
    }

    private Vector2 SpawnPos(int x, bool isVertical)
    {
        Vector2 spawnPos = Vector2.zero;
        if (isVertical) spawnPos = new Vector2(-80, 450 - x);
        else spawnPos = new Vector2(-750 + x, 80);

        return spawnPos;
    }

    private void SpawnButton(Vector2 pos)
    {
        // Spawns from pool and sets all positions correctly
        var buttonObject = pooler.SpawnFromPool("Button", Vector2.zero, Quaternion.identity);
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();

        buttonObject.transform.SetParent(scrollPanel);

        buttonRect.anchoredPosition = pos; buttonRect.localScale = Vector3.one;
        buttonList.Add(buttonObject.GetComponent<ButtonBehaviour>());
    }
    private void AssignButtons()
    {
        // Starting from the second row because first row is invisible by the mask
        int i = 2;

        // Finds and assigns buyable buildings scriptable objects from Object Manager 
        foreach (var unitObject in UnitObjectManager.UnitObjects)
        {
            BuildingObject building = unitObject as BuildingObject;

            if (!building) continue;

            if (building.buyable)
            {
                buttonList[i].building = building;
                i++;
            }
        }
        foreach (ButtonBehaviour button in buttonList)
        {
            button.leftPanelManager = this;
        }
    }


    private void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (!CheckIfOnCanvas()) return;

        if (IsWideScreen) scrollPosition = scrollPanel.anchoredPosition.y;
        else scrollPosition = scrollPanel.anchoredPosition.x;

        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            deltaPos = Input.mousePosition - firstPos;

            Vector3 movePos = scrollPanel.position;

            // Swerve
            if (IsWideScreen) movePos.y = Mathf.Lerp(movePos.y, movePos.y + (deltaPos.y / Screen.height) * scrollSpeed, Time.deltaTime * scrollSpeed);
            else movePos.x = Mathf.Lerp(movePos.x, movePos.x + (deltaPos.x / Screen.width) * scrollSpeed, Time.deltaTime * scrollSpeed);

            /* Clamp can be added if desired 
              movePos.y = Mathf.Clamp(movePos.y, 0, Mathf.Infinity);
            */

            scrollPanel.position = movePos;
            firstPos = Input.mousePosition;

            CheckIfScrolled();
        }
    }

    private void CheckIfScrolled()
    {
        // Checks if scrolled enough to spawn new buttons
        if (IsWideScreen)
        {
            if (lastScrollPosition < scrollPosition - 150)
            {
                float direction = BottomButton.BottomOfButton;
                ManageInfiniteScroll(TopButton, direction);
                ManageInfiniteScroll(TopButton, direction);
                lastScrollPosition += 150;
            }
            else if (lastScrollPosition >= scrollPosition + 150)
            {
                float direction = TopButton.TopOfButton;
                ManageInfiniteScroll(BottomButton, direction);
                ManageInfiniteScroll(BottomButton, direction);
                lastScrollPosition -= 150;
            }
        }
        else
        {
            if (lastScrollPosition < scrollPosition - 150)
            {
                float direction = LeftButton.LeftOfButton;
                ManageInfiniteScroll(RightButton, direction);
                ManageInfiniteScroll(RightButton, direction);
                lastScrollPosition += 150;
            }
            else if (lastScrollPosition >= scrollPosition + 150)
            {
                float direction = RightButton.RightOfButton;
                ManageInfiniteScroll(LeftButton, direction);
                ManageInfiniteScroll(LeftButton, direction);
                lastScrollPosition -= 150;
            }
        }
    }

    void ManageInfiniteScroll(ButtonBehaviour removedButton, float direction)
    {
        // Finds position to spawn
        Vector3 pos = removedButton.rectTransform.anchoredPosition;

        if (IsWideScreen) pos.y = direction;
        else pos.x = direction;

        buttonList.Remove(removedButton);
        pooler.DestroyPoolObject(removedButton.gameObject);

        SpawnButton(pos);
    }
    private bool CheckIfOnCanvas()
    {
        // Checks if mouse is on top of canvas
        RectTransform rectTransform = scrollPanel.parent.GetComponent<RectTransform>();

        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (rectTransform.rect.Contains(localMousePosition))
        {
            return true;
        }

        return false;
    }

    #region EdgeButtons
    // Finds buttons that are on all edges 
    public ButtonBehaviour TopButton
    {
        get
        {
            ButtonBehaviour highestValue = buttonList[0];
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].TopOfButton > highestValue.TopOfButton)
                {
                    highestValue = buttonList[i];
                }
            }
            return highestValue;
        }
    }
    public ButtonBehaviour BottomButton
    {
        get
        {
            ButtonBehaviour lowestValue = buttonList[0];
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].BottomOfButton < lowestValue.BottomOfButton)
                {
                    lowestValue = buttonList[i];
                }
            }
            return lowestValue;
        }
    }
    public ButtonBehaviour RightButton
    {
        get
        {
            ButtonBehaviour highestValue = buttonList[0];
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].RightOfButton > highestValue.RightOfButton)
                {
                    highestValue = buttonList[i];
                }
            }
            return highestValue;
        }
    }
    public ButtonBehaviour LeftButton
    {
        get
        {
            ButtonBehaviour lowestValue = buttonList[0];
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].LeftOfButton < lowestValue.LeftOfButton)
                {
                    lowestValue = buttonList[i];
                }
            }
            return lowestValue;
        }
    }
    #endregion
}
