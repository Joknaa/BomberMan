using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OknaaEXTENSIONS {
    public static class Extensions {
        
        /// <summary>
        /// Takes a random element of a list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Random<T>(this IList<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Converts an IEnumerable to a List
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<T> source) {
            return source != null ? new List<T>(source) : new List<T>();
        }


        public static List<Scene> GetAllLoadedScenes(this SceneManager sceneManager) {
            int countLoaded = SceneManager.sceneCount;
            List<Scene> loadedScenes = new List<Scene>();
 
            for (int i = 0; i < countLoaded; i++) {
                loadedScenes.Add(SceneManager.GetSceneAt(i));
            }

            return loadedScenes;
        }
        
        public static List<GameObject> GetAllChildren(this GameObject parent)  {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform) {
                children.Add(child.gameObject);
            }
            return children;
        }

        /// <summary>
        /// Fills the list with int values from start to end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IList<int> AddRange<T>(this IList<int> list, int start, int end) {
            for (int i = start; i < end; i++) {
                list.Add(i);
            }
            return list;
        }

        /// <summary>
        /// Removes the elements of one list from another list
        /// </summary>
        /// <param name="list">this list we are gonna remove from</param>
        /// <param name="subtractedList">the list we are gonna remove</param>
        /// <returns></returns>
        public static IList<T> Subtract<T>(this IList<T> list, IList<T> subtractedList) {
            List<T> newList = new List<T>();
            foreach (var element in list) {
                if (!subtractedList.Contains(element)) newList.Add(element);
            }
            return newList;
        }
        
    }
}