using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class WeightedInteger: WeightedValue {
        public int value;
        public int weighting;

        public int getValue()
        {
            return value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }

        public int getWeighting()
        {
            return weighting;
        }

        public void setWeighting(int weighting)
        {
            this.weighting = weighting;
        }
    }
}
