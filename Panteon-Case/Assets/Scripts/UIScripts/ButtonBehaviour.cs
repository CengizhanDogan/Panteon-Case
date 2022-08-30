using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class ButtonBehaviour : MonoBehaviour
{
    private ObjectPooler pooler;
    public BuildingPanel leftPanelManager;
    public BuildingObject building;

    [SerializeField] private Image image;

    private Button button;

    public RectTransform rectTransform;
    public float TopOfButton => rectTransform.anchoredPosition.y + 150;
    public float BottomOfButton => rectTransform.anchoredPosition.y - 150;
    public float RightOfButton => rectTransform.anchoredPosition.x + 150;
    public float LeftOfButton => rectTransform.anchoredPosition.x - 150;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        pooler = ObjectPooler.Instance;
    }

    private void Start()
    {
        if (building)
        {
            button.onClick.AddListener(delegate { BuyBuilding(); });
            image.sprite = building.visual;
        }
        else
        {
            Destroy(image);
            button.interactable = false;
        }
    }
    public void BuyBuilding()
    {
        var boughtObject = pooler.SpawnFromPool(building.unitName, InputManager.MousePosition, Quaternion.identity);
        EventManager.OnBuildingBought.Invoke(boughtObject.GetComponent<SpriteRenderer>(), building.cost);
        boughtObject.GetComponentInChildren<IPlaceableBuilding>().Move();
    }
}
