using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Buff
{
    [CreateAssetMenu(menuName = "Buff/Container")]
    public class BuffContainer : ScriptableObject
    {
        public List<BuffContainerData> data;
    }
    [System.Serializable]
    public class BuffContainerData
    {
        public BuffTarget Target;
        public List<BuffModificator> Modificators;
    }
}