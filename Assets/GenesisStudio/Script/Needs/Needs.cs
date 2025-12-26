using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace GenesisStudio
{
    public static class Needs
    {
        public static readonly string UIMap = "UI";
        public static readonly string PlayerMap = "Player";
        
        public static readonly string Move = "Move";
        public static readonly string Look = "Look";
        public static readonly string Interact = "Interact";
        public static readonly string Submit = "Submit";
        public static readonly string Use = "UseItem";
        public static readonly string Cancel = "Cancel";
        public static readonly string Fire = "Fire";
        public static readonly string Rotate = "Rotate";
        public static readonly string Player_Select = "SelectItem";
        public static readonly string Player_Sprint = "Sprint";

        
        public static readonly string TYPE_NONE = "None";
        public static readonly string TYPE_KEY = "Key";
        public static readonly string TYPE_STORAGE = "Storage";
        public static readonly string TYPE_FLUID_CONTAINER = "FluidContainer";
        
        public static string[] type = new string[]
        {
            TYPE_NONE,
            TYPE_KEY,
            TYPE_STORAGE,
            TYPE_FLUID_CONTAINER,
            
        };


        public static bool IsNearThePoint(this Transform t, Transform point)
        {
            float dis = Vector3.Distance(t.position, point.position);
            return dis <= 3.6f;
        }

        public static bool IsSameTypeAs<T>(object obj)
        {
            return obj.GetType() == typeof(T);
        }
        
        public static void LogColored(string message, string color = "white")
        {
            Debug.Log($"<color={color.ToLower()}>{message}</color>");
        }

        public static T GetRandomItemFromList<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        
        public static IEnumerator DestroyItemsAndClearTheList<T>(this List<T> list) where T : Component
        {
            var copy = list;
            if (copy.Count == 0) yield break;
            foreach (T item in copy)
            {
                MonoBehaviour.Destroy(item.gameObject);
                yield return null;
            }
            copy.Clear();
        }

        public static int GetRandomIndexFromList<T>(this IList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static T GetNearestObject<T>(this IList<T> list, Transform currentPostition) where T : MonoBehaviour
        {
            T nearestObject = null;
            if (list.Count == 0) return null;
            float nearestDistance = float.MaxValue;
            
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i] == null) continue;

                float distance = Vector3.Distance(currentPostition.position, list[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestObject = list[i];
                }
            }

            return nearestObject;
        }

        public static GameObject AddIndicator(this Transform target, float indicator_height, Color indicator_color)
        {
            var emptyGO = new GameObject("Waypoint Indicator");
            emptyGO.transform.SetParent(target);

            emptyGO.transform.localPosition = target.transform.up * indicator_height;
            var indicator = emptyGO.AddComponent<Target>();
            indicator.TargetColor = indicator_color;

            return emptyGO; 
        }

        public static GameObject AddIndicator(this Transform target)
        {
            return target.AddIndicator(GameManager.Instance.indicator_height, GameManager.Instance.indicator_color);
        }

        public static bool CanAIInteractWithPoint(this Transform point)
        {
            point.TryGetComponent(out AIInteractable interactable);
            LogColored($"Cheking :{interactable}");
            return interactable != null && !interactable.interacted;
        }
        
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
        
    }
}

