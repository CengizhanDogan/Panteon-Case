using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CraftingPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image craftingImage;
    [SerializeField] private Button craftButton;

    public void SetButton(Unit unit,Vector3 spawnPos, string text, Sprite sprite)
    {
        craftButton.onClick.AddListener(delegate { unit.CreateObjects(spawnPos); });
        textMesh.text = text;
        craftingImage.sprite = sprite;
    }
}
