using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PFX
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager I { get; private set; }

        public TMP_Text currentPowerText;



        private void Awake()
        {
            I = this;
        }

        private void Update()
        {
            currentPowerText.text = "Current Power = " + GameManager.I.currentPower;
        }
    }
}
