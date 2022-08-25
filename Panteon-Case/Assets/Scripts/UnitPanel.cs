using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject craftPanel;

    private List<GameObject> craftPanels = new List<GameObject>();

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
        ResetPanel();

        panel.SetActive(true);

        SetCraftingPanel(unit as Building, transform);

        UnitAttributes attributes = unit.attributes;

        unitName.text = attributes.unitName;
        healthAttribute.text = $"Health: {attributes.unitHealth}";
        damageAttribute.text = $"Damage: {attributes.unitDamage}";
        speedAttribute.text = $"Speed: {attributes.unitSpeed}";

        unitImage.sprite = attributes.unitImage;
    }
    private void SetCraftingPanel(Building building, Transform transform)
    {
        if (!building)
        {
            craftingPanel.SetActive(false);
            return;
        }

        if (building.craftingList.Count > 0)
        {
            craftingPanel.SetActive(true);
        }

        int x = 1;
        int y = 0;
        for (int i = 0; i < building.craftingList.Count; i++)
        {
            if (i % 2 == 0)
            {
                y++;
            }
            else
            {
                x *= -1;
            }

            craftPanels.Add(Instantiate(craftPanel, craftingPanel.transform.position, Quaternion.identity, craftingPanel.transform));
            craftPanels[i].GetComponent<RectTransform>().localPosition = new Vector3(x * -100, y * -150, 0);
            craftPanels[i].GetComponent<CraftingPanel>().SetButton(building.craftingList[i], transform.position + Vector3.one * 0.5f, building.craftingList[i].unitName, building.craftingList[i].visual);

            Debug.Log(building.gfx, building.gfx);
        }
    }
    private void ResetPanel()
    {
        for (int i = 0; i < craftPanels.Count; i++)
        {
            var panel = craftPanels[0];
            craftPanels.Remove(panel);
            Destroy(panel);
        }

        panel.SetActive(false);
    }
}
