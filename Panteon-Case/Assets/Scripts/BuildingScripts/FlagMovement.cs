using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : BuildingMovement
{
    public override IEnumerator Follow(bool follow, int sortingLayer)
    {
        sortingLayer = 1;
        StartCoroutine(base.Follow(follow, sortingLayer));
        yield return null;
    }
}
