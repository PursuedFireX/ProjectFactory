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

        //Assets
        public Transform chatPopup;
        public GameObject chatMessage;

    }
}
