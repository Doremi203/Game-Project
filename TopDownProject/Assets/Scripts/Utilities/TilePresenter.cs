using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[SelectionBase]
public class TilePresenter : MonoBehaviour
{

    public int CurrentVariantIndex;
    public GameObject CurrentVariation;
    public Tile TargetTile;

    public void Setup(Tile target)
    {
        this.TargetTile = target;
        SelectVariation(0);
    }

    public void ResetTile() => SelectVariation(CurrentVariantIndex);

    public void SelectVariation(int index)
    {
        CurrentVariantIndex = index;

        GameObject _lastVariation = CurrentVariation;

        CurrentVariation = Instantiate(TargetTile.Variations[index], this.transform);

        CurrentVariation.transform.parent = this.transform;
        CurrentVariation.transform.position = transform.position;

        if (_lastVariation)
        {
            CurrentVariation.transform.localScale = _lastVariation.transform.localScale;
            CurrentVariation.transform.rotation = _lastVariation.transform.rotation;

            MeshRenderer _meshRenderCurrent = CurrentVariation.GetComponent<MeshRenderer>();
            MeshRenderer _meshRenderLast = _lastVariation.GetComponent<MeshRenderer>();
            _meshRenderCurrent.sharedMaterials = _meshRenderLast.sharedMaterials;

            DestroyImmediate(_lastVariation);
        }
    }

    public void FlipX()
    {
        CurrentVariation.transform.localScale = new Vector3(CurrentVariation.transform.localScale.x * -1, 1, 1);
    }

    public void Rotate90() => CurrentVariation.transform.Rotate(Vector3.up, 90f);

    public void RotateMinus90() => CurrentVariation.transform.Rotate(Vector3.up, -90f);

}

#endif