using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    public class Commands : MonoBehaviour
    {
        public static Commands I { get; private set; }

        private void Awake()
        {
            I = this;
        }

        public void AddPower(int amount)
        {
            GameManager.I.currentPower += amount;
        }

        public void SetPower(int amount)
        {
            GameManager.I.currentPower = amount;
        }

    }
}
