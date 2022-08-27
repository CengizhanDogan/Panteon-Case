using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public BuildingPanelManager leftPanelManager;
    public Building building;

    [SerializeField] private Image image;

    private Button button;

    private float topButtonValue;
    private float bottomButtonValue;

    public float TopOfButton
    {
        get
        {
            return transform.position.y + 2.778137f;
        }
    }
    public float BottomOfButton
    {
        get
        {
            return transform.position.y - 2.778137f;
        }
    }
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (building)
        {
            button.onClick.AddListener(delegate { ButtonPress(); });
            image.sprite = building.visual;
        }
        else
        {
            Destroy(image);
            button.interactable = false;
        }

        topButtonValue = leftPanelManager.TopButton.TopOfButton;
        bottomButtonValue = leftPanelManager.BottomButton.BottomOfButton;
    }
    private void Update()
    {
        if (leftPanelManager.GetListInt(this) % 2 == 0)
        {
            if (transform.position.y >= topButtonValue)
            {
                Vector3 bottomPos = transform.position; bottomPos.y = leftPanelManager.BottomButton.BottomOfButton;
                transform.position = bottomPos;
            }
            if (transform.position.y <= bottomButtonValue)
            {
                Vector3 topPos = transform.position; topPos.y = leftPanelManager.TopButton.TopOfButton;
                transform.position = topPos;
            }
        }
        else
        {
            Vector3 followPos = transform.position; followPos.y = leftPanelManager.ButtonTransform(leftPanelManager.GetListInt(this)).position.y;
            transform.position = followPos;

        }
    }
    public void ButtonPress()
    {
        building.CreateObjects(Camera.main.ScreenToWorldPoint(Input.mousePosition), 5, out var unit, false, null);
    }


}
