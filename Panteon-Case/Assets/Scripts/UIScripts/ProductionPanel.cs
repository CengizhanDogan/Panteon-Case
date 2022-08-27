using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image craftingImage;
    [SerializeField] private Button craftButton;

    public void SetButton(Unit unit,Vector3 spawnPos, string text, Sprite sprite, GameObject flag)
    {
        craftButton.onClick.AddListener(delegate { unit.CreateObjects(spawnPos, 2, out var unitObject, true, flag); });

        textMesh.text = text;
        craftingImage.sprite = sprite;
    }
}
