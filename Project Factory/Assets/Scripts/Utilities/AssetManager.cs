using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class AssetManager : MonoBehaviour
    {
        public static AssetManager I { get; private set; }

        private void Awake()
        {
            I = this;
        }

        [Header("Prefabs")]
        public GameObject cablePrefab;
        public Transform chatPopup;
        public GameObject chatMessage;
        [Space]
        public GameObject textPopup;

        [Header("Materials")]
        public Material powerLineMat;

    }
}
