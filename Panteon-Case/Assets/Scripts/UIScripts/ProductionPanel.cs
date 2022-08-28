using System.Collections;
using System.Collections.Generic;
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

    public void SetButton(UnitObject unit,Vector2 spawnPos, string text, Sprite sprite, Transform flag)
    {
        craftButton.onClick.AddListener(delegate { ProduceUnit(unit.unitName, spawnPos, flag); });
        textMesh.text = text;
        craftingImage.sprite = sprite;
    }

    private void ProduceUnit(string unitName, Vector2 spawnPos, Transform flag)
    {
        Pooler.SpawnFromPool(unitName, spawnPos, Quaternion.identity, out var product);
        UnitBehaviour behaviour = product.AddComponent<UnitBehaviour>();
        behaviour.unit = UnitObjectManager.GetUnit(unitName);
        behaviour.GetComponent<SpriteRenderer>().sortingOrder = 2;
        UnitMover.Instance.StartMovement(product, flag);
    }
}
