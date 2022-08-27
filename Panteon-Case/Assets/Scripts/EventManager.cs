using System;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static BuildingBoughtEvent OnBuildingBought = new BuildingBoughtEvent();

    public static GridMoveEvent OnGridMove = new GridMoveEvent();
    public static BuildingPlaceEvent OnBuildingPlace = new BuildingPlaceEvent();

    public static PlaceableEvent OnPlaceableEvent = new PlaceableEvent();
    public static UnavailableEvent OnUnavailableEvent = new UnavailableEvent();

    public static SelectionEvent OnSelectionEvent = new SelectionEvent();
    public static DeselectEvent OnDeselectEvent = new DeselectEvent();
}

public class BuildingBoughtEvent : UnityEvent<SpriteRenderer, int> { }
public class GridMoveEvent : UnityEvent<BuildingMovement, Vector2> { }
public class BuildingPlaceEvent : UnityEvent<BuildingMovement, Vector2, int> { }
public class PlaceableEvent : UnityEvent { }
public class UnavailableEvent : UnityEvent { }
public class SelectionEvent : UnityEvent<Unit, Transform> { }
public class DeselectEvent : UnityEvent<bool> { }
