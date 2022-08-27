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

    private void SetPanel(Unit unit, Transform transform)
    {
        ResetPanel(false);
        AnimatePanel();

        panel.SetActive(true);

        SetProductionPanel(unit as Building, transform);

        UnitAttributes attributes = unit.attributes;

        unitName.text = attributes.unitName;
        healthAttribute.text = $"Health: {attributes.unitHealth}";
        damageAttribute.text = $"Damage: {attributes.unitDamage}";
        speedAttribute.text = $"Speed: {attributes.unitSpeed}";

        unitImage.sprite = attributes.unitImage;
    }

    private void AnimatePanel()
    {
        DOTween.Kill(this);
        transform.DOMoveX(13.5f, 0.5f);
    }

    private void SetProductionPanel(Building building, Transform transform)
    {
        if (!building)
        {
            productionPanel.SetActive(false);
            return;
        }

        if (building.productionList.Count > 0)
        {
            productionPanel.SetActive(true);
        }

        int x = 1;
        int y = 0;
        for (int i = 0; i < building.productionList.Count; i++)
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
            productionPanels[i].GetComponent<RectTransform>().localPosition = new Vector3(x * -100, y * -150, 0);
            productionPanels[i].GetComponent<ProductionPanel>().SetButton(building.productionList[i], transform.position + Vector3.one * 0.5f, building.productionList[i].unitName, building.productionList[i].visual, building.flagObject);
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
        if (close) transform.DOMoveX(23, 0.5f);
        panel.SetActive(false);
    }
}
