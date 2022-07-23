using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StevieDev.DialogueEditor
{
    public class GameEvents : MonoBehaviour
    {
        private event Action _randomColorModel;
        private static GameEvents _instance;

        public static GameEvents Instance { get => _instance; }
        public Action RandomColorModel { get => _randomColorModel; set => _randomColorModel = value; }

        private void Awake()
        {
            _instance = this;
        }

        public void CallRandomColorModel()
        {
            _randomColorModel?.Invoke();
        }
    }
}