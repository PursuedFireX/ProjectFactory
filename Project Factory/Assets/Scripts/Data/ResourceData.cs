using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFX
{
    [CreateAssetMenu(menuName = "Data/Resource Data")]
    public class ResourceData : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
    }
}
