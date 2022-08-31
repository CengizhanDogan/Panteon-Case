using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class ProductionPanel : MonoBehaviour
{
    private ObjectPooler Pooler => ObjectPooler.Instance;

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image craftingImage;
    [SerializeField] private Button craftButton;
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
    public void SetButton(UnitObject unit,Vector2 spawnPos, string text, Sprite sprite, Transform flag)
    {
        craftButton.onClick.AddListener(delegate { ProduceUnit(unit.unitName, spawnPos, flag); });

        textMesh.text = text;
        craftingImage.sprite = sprite;
    }

    private void ProduceUnit(string unitName, Vector2 spawnPos, Transform flag)
    {
        if (!canInteract || !flag.gameObject.activeSelf) return;

        // Finds unit by given name in the Object Pooler
        var product = Pooler.SpawnFromPool(unitName, spawnPos, Quaternion.identity);

        UnitBehaviour behaviour = product.AddComponent<UnitBehaviour>();

        // Sets unit scriptible object of the created object 
        behaviour.unit = UnitObjectManager.GetUnit(unitName);

        behaviour.GetComponent<SpriteRenderer>().sortingOrder = 2;
        // Starts the first movement that unit goes to its buildings flag
        UnitMover.Instance.StartMovement(product, flag);
    }
}
