using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class WeightedValueSelector
    {
        public static WeightedValue selectValue(WeightedValue[] weightedValues) {
            Dictionary<int, WeightedValue> weightBoundaries = new Dictionary<int, WeightedValue>();

            int totalWeight = 0;

            //store list of boundaries for the matching weighted value
            for(int i = 0; i < weightedValues.Length; i++) {
                if(weightedValues[i].getWeighting() > 0) { 
                    totalWeight += weightedValues[i].getWeighting();
                    weightBoundaries.Add(totalWeight, weightedValues[i]);
                }                
            }

            //generate random number between 0 and the total weight
            float value = Random.Range(0, totalWeight);

            //return the weighted value whose boundary exceeds the randomly generated value
            foreach(KeyValuePair<int, WeightedValue> entry in weightBoundaries) {
                // do something with entry.Value or entry.Key
                if(value <= entry.Key) {
                    return entry.Value;
                }
            }

            return null;
        }
    }
}
