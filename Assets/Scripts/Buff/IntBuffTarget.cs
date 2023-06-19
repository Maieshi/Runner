using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(menuName = "Buff/Target/Int")]
    public class IntBuffTarget : BuffTarget
    {
        public override void AddModificator(BuffModificator modificator)
        {
            base.AddModificator(modificator);
            _resultValue = Mathf.RoundToInt(_resultValue);
        }
        public override void RemoveModificator(BuffModificator modificator)
        {
            base.AddModificator(modificator);
            _resultValue = Mathf.RoundToInt(_resultValue);
        }


    }
}