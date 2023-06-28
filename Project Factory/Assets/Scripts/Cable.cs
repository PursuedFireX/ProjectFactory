using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NOT_Lonely;

namespace PFX
{
    public class Cable : MonoBehaviour
    {
        private bool placed;
        [SerializeField] private ACC_Trail cable;

        public void StartLine(Transform point)
        {
            cable.controlPoints.Add(point);
            cable.controlPoints.Add(GameManager.I.transform);
            cable.UpdateCableTrail();
        }


        private void Update()
        {
            if(!placed)
            {
                cable.UpdateCableTrail();
            }
        }

        public void EndLine(Transform point)
        {
            placed = true;
            cable.controlPoints[1] = point;
            cable.UpdateCableTrail();
        }

    }
}
