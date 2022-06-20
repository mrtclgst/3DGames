using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    [System.Serializable]
    public class ShopElement : MonoBehaviour
    {
        public string name;
        public int index, price;
        public bool isLocked;
    }
}
