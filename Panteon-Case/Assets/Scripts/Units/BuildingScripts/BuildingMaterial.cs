using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMaterial : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Material placeableMat;
    [SerializeField] private Material unavailableMat;
    private Material defaultMat;

    private void OnEnable()
    {
        EventManager.OnBuildingBought.AddListener(SetRenderer);
        EventManager.OnPlaceableEvent.AddListener(ChangeToPlaceable);
        EventManager.OnUnavailableEvent.AddListener(ChangeToUnavailable);
        EventManager.OnBuildingPlace.AddListener(ChangeToDefault);
    }
    private void OnDisable()
    {
        EventManager.OnBuildingBought.RemoveListener(SetRenderer);
        EventManager.OnPlaceableEvent.RemoveListener(ChangeToPlaceable);
        EventManager.OnUnavailableEvent.RemoveListener(ChangeToUnavailable);
        EventManager.OnBuildingPlace.RemoveListener(ChangeToDefault);
    }
    private void SetRenderer(SpriteRenderer rend, int i)
    {
        spriteRenderer = rend;
        defaultMat = spriteRenderer.sharedMaterial;
        spriteRenderer.sortingOrder = 5;
    }

    private void ChangeToPlaceable()
    {
        spriteRenderer.material = placeableMat;
    }
    private void ChangeToUnavailable()
    {
        spriteRenderer.material = unavailableMat;
    }
    private void ChangeToDefault(BuildingMovement b, Vector2 v, int sortingOrder)
    {
        spriteRenderer.material = defaultMat;
        spriteRenderer.sortingOrder = sortingOrder;
    }
}
