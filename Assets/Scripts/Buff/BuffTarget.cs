using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buff
{
    [CreateAssetMenu(menuName = "Buff/Target/Float")]
    public class BuffTarget : ScriptableObject
    {
        [SerializeField]
        protected float _startValue;

        // public float vaaal;

        protected float _resultValue;

        public float Value => _resultValue;

        public virtual void AddModificator(BuffModificator modificator)
        {
            if (modificator.Operation == OperationType.Add) _resultValue += modificator.Value;
            else _resultValue *= modificator.Value;

        }

        public virtual void RemoveModificator(BuffModificator modificator)
        {
            if (modificator.Value == 0)
            {
                ResetResultValue();
                return;
            }
            if (modificator.Operation == OperationType.Add) _resultValue -= modificator.Value;
            else _resultValue /= modificator.Value;
        }
        // public virtual float TargetValue
        // {
        //     get
        //     {
        //         float val;
        //         Debug.Log("//" + _startValue);
        //         if (!_modificator) val = _startValue;
        //         else
        //         {
        //             val = (_modificator.Operation == OperationType.Add) ? _startValue + _modificator.ModificatorValue : _startValue * _modificator.ModificatorValue;
        //         }
        //         Debug.Log(val + "//" + _startValue);
        //         return val;
        //     }
        // }

        // protected BuffModificator _modificator;


        // public virtual BuffModificator Modificator
        // {
        //     set
        //     {
        //         if (value.Type == ModificatorType.Permanent)
        //             _startValue = (value.Operation == OperationType.Add) ? _startValue + value.ModificatorValue : _startValue * value.ModificatorValue;
        //         else
        //             _modificator = value;
        //     }
        // }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public void ResetResultValue()
        {
            // Debug.Log("reset" + this.name);
            _resultValue = _startValue;
        }
    }
}