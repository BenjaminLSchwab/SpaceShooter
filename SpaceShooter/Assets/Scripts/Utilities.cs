using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public static class Utilities : object
    {
        public static GameObject PullFromPool(List<GameObject> pool)
        {
        if (pool.Count == 0)
        {
            return null;
        }
        foreach (var gameObj in pool)
            {
                if (!gameObj.activeInHierarchy)
                {
                    return gameObj;
                }

            }
            return null;
        }
    }




