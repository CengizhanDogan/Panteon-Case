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

    // Button directions for infinite scrollview 
    public float TopOfButton => rectTransform.anchoredPosition.y + 150;
    public float BottomOfButton => rectTransform.anchoredPosition.y - 150;
    public float RightOfButton => rectTransform.anchoredPosition.x + 150;
    public float LeftOfButton => rectTransform.anchoredPosition.x - 150;
    //

    private bool canInteract = true;
    private void OnEnable()
    {
        EventManager.OnBuildingBought.AddListener((SpriteRenderer r, int i) => canInteract = false);
        EventManager.OnBuildingPlace.AddListener((BuildingMovement b, Vector2 v, int i) => canInteract = true);
        EventManager.OnCancel.AddListener(() => canInteract = true);
    }
    private void OnDisable()
    {
        EventManager.OnBuildingBought.RemoveListener((SpriteRenderer r, int i) => canInteract = false);
        EventManager.OnBuildingPlace.RemoveListener((BuildingMovement b, Vector2 v, int i) => canInteract = true);
        EventManager.OnCancel.RemoveListener(() => canInteract = true);
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        pooler = ObjectPooler.Instance;
    }

    private void Start()
    {
        // Checks if button has building to produce
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
        if (!canInteract) return;

        var boughtObject = pooler.SpawnFromPool(building.unitName, InputManager.MousePosition, Quaternion.identity);

        EventManager.OnBuildingBought.Invoke(boughtObject.GetComponent<SpriteRenderer>(), building.cost);
        // Starts building movement at spawn from pool.
        boughtObject.GetComponentInChildren<IPlaceableBuilding>().Move();
    }
}
