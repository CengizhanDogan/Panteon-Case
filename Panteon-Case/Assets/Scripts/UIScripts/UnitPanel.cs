using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject productionPanel;
    [SerializeField] private GameObject craftPanel;

    private List<GameObject> productionPanels = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI healthAttribute;
    [SerializeField] private TextMeshProUGUI damageAttribute;
    [SerializeField] private TextMeshProUGUI speedAttribute;

    [SerializeField] private Image unitImage;

    [SerializeField] private bool isVertical;

    private void OnEnable()
    {
        EventManager.OnSelectionEvent.AddListener(SetPanel);
        EventManager.OnDeselectEvent.AddListener(ResetPanel);
    }

    private void OnDisable()
    {
        EventManager.OnSelectionEvent.RemoveListener(SetPanel);
        EventManager.OnDeselectEvent.RemoveListener(ResetPanel);
    }

    private void SetPanel(UnitBehaviour unitBehaviour, Transform transform)
    {
        ResetPanel(false);

        panel.SetActive(true);

        SetProductionPanel(unitBehaviour as BuildingBehaviour, transform);

        UnitAttributes attributes = unitBehaviour.unit.attributes;

        unitName.text = unitBehaviour.unit.unitName;
        healthAttribute.text = $"Health: {attributes.unitHealth}";
        damageAttribute.text = $"Damage: {attributes.unitDamage}";
        speedAttribute.text = $"Speed: {attributes.unitSpeed}";

        unitImage.sprite = attributes.unitImage;
    }

    private void SetProductionPanel(BuildingBehaviour building, Transform transform)
    {
        if (!building)
        {
            productionPanel.SetActive(false);
            return;
        }

        BuildingObject buildingObject = building.unit as BuildingObject;

        if (buildingObject.productionList.Count > 0)
        {
            productionPanel.SetActive(true);
        }
        else
        {
            productionPanel.SetActive(false);
            return;
        }

        if (isVertical)
        {

            int x = 1;
            int y = 0;
            for (int i = 0; i < buildingObject.productionList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    y++;
                }
                else
                {
                    x *= -1;
                }

                productionPanels.Add(Instantiate(craftPanel, productionPanel.transform.position, Quaternion.identity, productionPanel.transform));
                productionPanels[i].GetComponent<RectTransform>().localPosition = new Vector2(x * -100, y * -150);
                productionPanels[i].GetComponent<ProductionPanel>().SetButton(buildingObject.productionList[i], transform.position + Vector3.one * 0.5f, buildingObject.productionList[i].unitName, buildingObject.productionList[i].visual, building.flagTransform);
            }
        }
        else
        {
            for (int i = 0; i < buildingObject.productionList.Count; i++)
            {
                productionPanels.Add(Instantiate(craftPanel, productionPanel.transform.position, Quaternion.identity, productionPanel.transform));
                productionPanels[i].GetComponent<RectTransform>().localPosition = new Vector2(i * 200, -200);
                productionPanels[i].GetComponent<ProductionPanel>().SetButton(buildingObject.productionList[i], transform.position + Vector3.one * 0.5f, buildingObject.productionList[i].unitName, buildingObject.productionList[i].visual, building.flagTransform);
            }
        }
    }
    private void ResetPanel(bool close)
    {
        for (int i = 0; i < productionPanels.Count; i++)
        {
            var panel = productionPanels[0];
            productionPanels.Remove(panel);
            Destroy(panel);
        }

        panel.SetActive(false);
    }
}
