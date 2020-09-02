﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TilePresenter : MonoBehaviour
{

    public Tile TargetTile;
    public int CurrentVariantIndex;

    private GameObject currentVariation;

    public void Setup(Tile target)
    {
        this.TargetTile = target;
        SelectVariation(0);
    }

    public void SelectVariation(int index)
    {
        CurrentVariantIndex = index;

        GameObject _lastVariation = currentVariation;

        currentVariation = Instantiate(TargetTile.Variations[index], this.transform);

        currentVariation.transform.position = transform.position;

        if (_lastVariation)
        {
            currentVariation.transform.localScale = _lastVariation.transform.localScale;
            currentVariation.transform.rotation = _lastVariation.transform.rotation;
            DestroyImmediate(_lastVariation);
        }
    }

    public void FlipX()
    {
        currentVariation.transform.localScale = new Vector3(currentVariation.transform.localScale.x * -1, 1, 1);
    }

    public void Rotate90()
    {
        currentVariation.transform.Rotate(Vector3.up, 90f);
    }

    public void RotateMinus90()
    {
        currentVariation.transform.Rotate(Vector3.up, -90f);
    }

}