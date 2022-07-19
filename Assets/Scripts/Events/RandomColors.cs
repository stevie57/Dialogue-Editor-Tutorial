using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColors : MonoBehaviour
{
    private List<Material> _materials = new List<Material>();

    private void Awake()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (Material mat in skinnedMeshRenderer.materials) 
            {
                _materials.Add(mat);
            }
        }
    }

    private void Start()
    {
        GameEvents.Instance.RandomColorModel += DoRandomColorModel;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.RandomColorModel -= DoRandomColorModel;
    }

    private void DoRandomColorModel()
    {
        foreach(Material mat in _materials)
        {
            mat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
}
