using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace Nora
{
    public class Collections : MonoBehaviour
    {
        public static T GetRandomFromList<T>(List<T> list)
        {
            var index = Random.Range(0, list.Count);
            return list[index];
        }

        public static T GetRandomWithChance<T>(Dictionary<T, int> objectChanceMap)
        {
            try
            {
                var chancesSum = objectChanceMap.Values.ToList().Sum();
                float randomChance = Random.Range(0, chancesSum);

                float totalChance = 0;
                foreach (KeyValuePair<T, int> pair in objectChanceMap)
                {
                    totalChance += pair.Value;
                    if (totalChance > randomChance)
                        return pair.Key;
                }
            }
            catch
            {
            }

            return default;
        }
    }
}
