using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private LanguageType _language;

    private static LanguageController _instance;
    public static LanguageController Instance { get => _instance;}
    public LanguageType Language { get => _language; set => _language = value; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
