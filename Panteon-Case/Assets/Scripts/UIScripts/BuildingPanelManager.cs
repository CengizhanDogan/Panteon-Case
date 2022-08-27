using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPanelManager : MonoBehaviour
{
    [SerializeField] private List<Building> buildingList = new List<Building>();
    [SerializeField] private List<ButtonBehaviour> buttonList = new List<ButtonBehaviour>();

    [SerializeField] private RectTransform scrollObject;
    [SerializeField] private float scrollSpeed;
    private Vector3 deltaPos;
    private Vector3 firstPos;

    private void Awake()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buttonList[i].building = buildingList[i];
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
        if (!CheckIfOnCanvas) return;

        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            deltaPos = Input.mousePosition - firstPos;

            Vector3 movePos = scrollObject.position;
            movePos.y = Mathf.Lerp(movePos.y, movePos.y + (deltaPos.y / Screen.width) * scrollSpeed, Time.deltaTime * scrollSpeed);

            //Clamp can be added if desired 
            //movePos.y = Mathf.Clamp(movePos.y, 0, Mathf.Infinity);

            scrollObject.position = movePos;
            firstPos = Input.mousePosition;
        }
    }
    private bool CheckIfOnCanvas
    {
        get
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 localMousePosition = rectTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (rectTransform.rect.Contains(localMousePosition))
            {
                return true;
            }

            return false;
        }
    }

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

    public int GetListInt(ButtonBehaviour button)
    {
        return buttonList.IndexOf(button);
    }

    public Transform ButtonTransform(int listInt)
    {
        return buttonList[listInt - 1].transform;
    }
}
