namespace UtageExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public static class UtageExtensions
    {
        public static GameObject AddChild(this Transform t, GameObject child)
        {
            return t.AddChild(child, Vector3.get_zero(), Vector3.get_one());
        }

        public static GameObject AddChild(this Transform t, GameObject child, Vector3 localPosition)
        {
            return t.AddChild(child, localPosition, Vector3.get_one());
        }

        public static GameObject AddChild(this Transform t, GameObject child, Vector3 localPosition, Vector3 localScale)
        {
            child.get_transform().SetParent(t);
            child.get_transform().set_localScale(localScale);
            child.get_transform().set_localPosition(localPosition);
            if (child.get_transform() is RectTransform)
            {
                (child.get_transform() as RectTransform).set_anchoredPosition(localPosition);
            }
            child.get_transform().set_localRotation(Quaternion.get_identity());
            child.ChangeLayerDeep(t.get_gameObject().get_layer());
            return child;
        }

        public static GameObject AddChildGameObject(this Transform t, string name)
        {
            return t.AddChildGameObject(name, Vector3.get_zero(), Vector3.get_one());
        }

        public static GameObject AddChildGameObject(this Transform t, string name, Vector3 localPosition)
        {
            return t.AddChildGameObject(name, localPosition, Vector3.get_one());
        }

        public static GameObject AddChildGameObject(this Transform t, string name, Vector3 localPosition, Vector3 localScale)
        {
            GameObject child = !(t is RectTransform) ? new GameObject(name) : new GameObject(name, new Type[] { typeof(RectTransform) });
            t.AddChild(child, localPosition, localScale);
            return child;
        }

        public static T AddChildGameObjectComponent<T>(this Transform t, string name) where T: Component
        {
            return t.AddChildGameObjectComponent<T>(name, Vector3.get_zero(), Vector3.get_one());
        }

        public static T AddChildGameObjectComponent<T>(this Transform t, string name, Vector3 localPosition) where T: Component
        {
            return t.AddChildGameObjectComponent<T>(name, localPosition, Vector3.get_one());
        }

        public static T AddChildGameObjectComponent<T>(this Transform t, string name, Vector3 localPosition, Vector3 localScale) where T: Component
        {
            GameObject obj2 = t.AddChildGameObject(name, localPosition, localScale);
            T component = obj2.GetComponent<T>();
            if (component == null)
            {
                return obj2.AddComponent<T>();
            }
            return component;
        }

        public static GameObject AddChildPrefab(this Transform t, GameObject prefab)
        {
            return t.AddChildPrefab(prefab, Vector3.get_zero(), Vector3.get_one());
        }

        public static GameObject AddChildPrefab(this Transform t, GameObject prefab, Vector3 localPosition)
        {
            return t.AddChildPrefab(prefab, localPosition, Vector3.get_one());
        }

        public static GameObject AddChildPrefab(this Transform t, GameObject prefab, Vector3 localPosition, Vector3 localScale)
        {
            GameObject go = Object.Instantiate<GameObject>(prefab, t);
            go.get_transform().set_localScale(localScale);
            go.get_transform().set_localPosition(localPosition);
            go.ChangeLayerDeep(t.get_gameObject().get_layer());
            return go;
        }

        public static GameObject AddChildUnityObject(this Transform t, Object obj)
        {
            return (Object.Instantiate(obj, t) as GameObject);
        }

        public static void ChangeLayerDeep(this GameObject go, int layer)
        {
            go.set_layer(layer);
            IEnumerator enumerator = go.get_transform().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ((Transform) enumerator.Current).get_gameObject().ChangeLayerDeep(layer);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public static RenderTexture CreateCopyTemporary(this RenderTexture renderTexture)
        {
            return renderTexture.CreateCopyTemporary(renderTexture.get_depth());
        }

        public static RenderTexture CreateCopyTemporary(this RenderTexture renderTexture, int depth)
        {
            RenderTexture texture = RenderTexture.GetTemporary(renderTexture.get_width(), renderTexture.get_height(), depth, renderTexture.get_format());
            Graphics.Blit(renderTexture, texture);
            return texture;
        }

        public static void DestroyChildren(this Transform t)
        {
            List<Transform> list = new List<Transform>();
            IEnumerator enumerator = t.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    current.get_gameObject().SetActive(false);
                    list.Add(current);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            t.DetachChildren();
            foreach (Transform transform2 in list)
            {
                Object.Destroy(transform2.get_gameObject());
            }
        }

        public static void DestroyChildrenInEditorOrPlayer(this Transform t)
        {
            List<Transform> list = new List<Transform>();
            IEnumerator enumerator = t.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    current.get_gameObject().SetActive(false);
                    list.Add(current);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            t.DetachChildren();
            foreach (Transform transform2 in list)
            {
                if (Application.get_isPlaying())
                {
                    Object.Destroy(transform2.get_gameObject());
                }
                else
                {
                    Object.DestroyImmediate(transform2.get_gameObject());
                }
            }
        }

        public static T Find<T>(this Transform t, string name) where T: Component
        {
            Transform transform = t.Find(name);
            if (transform == null)
            {
                return null;
            }
            return transform.GetComponent<T>();
        }

        public static Transform FindDeep(this Transform t, string name, bool includeInactive = false)
        {
            foreach (Transform transform in t.GetComponentsInChildren<Transform>(includeInactive))
            {
                if (transform.get_gameObject().get_name() == name)
                {
                    return transform;
                }
            }
            return null;
        }

        public static T FindDeepAsComponent<T>(this Transform t, string name, bool includeInactive = false) where T: Component
        {
            foreach (T local in t.GetComponentsInChildren<T>(includeInactive))
            {
                if (local.get_gameObject().get_name() == name)
                {
                    return local;
                }
            }
            return null;
        }

        public static float GetAlpha(this Graphic graphic)
        {
            return graphic.get_color().a;
        }

        public static T GetCompoentInChildrenCreateIfMissing<T>(this Transform t) where T: Component
        {
            return t.GetCompoentInChildrenCreateIfMissing<T>(typeof(T).Name);
        }

        public static T GetCompoentInChildrenCreateIfMissing<T>(this Transform t, string name) where T: Component
        {
            T componentInChildren = t.GetComponentInChildren<T>();
            if (componentInChildren == null)
            {
                componentInChildren = t.AddChildGameObjectComponent<T>(name);
            }
            return componentInChildren;
        }

        public static T GetComponentCache<T>(this Component target, ref T component) where T: class
        {
            try
            {
                return target.get_gameObject().GetComponentCache<T>(ref component);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return null;
            }
        }

        public static T GetComponentCache<T>(this GameObject go, ref T component) where T: class
        {
            T local;
            if (((T) component) == null)
            {
                component = local = go.GetComponent<T>();
            }
            return local;
        }

        public static T GetComponentCacheCreateIfMissing<T>(this Component target, ref T component) where T: Component
        {
            try
            {
                return target.get_gameObject().GetComponentCacheCreateIfMissing<T>(ref component);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return null;
            }
        }

        public static T GetComponentCacheCreateIfMissing<T>(this GameObject go, ref T component) where T: Component
        {
            T local;
            if (((T) component) == null)
            {
                component = local = go.GetComponentCreateIfMissing<T>();
            }
            return local;
        }

        public static T GetComponentCacheFindIfMissing<T>(this Component target, ref T component) where T: Component
        {
            return target.get_gameObject().GetComponentCacheFindIfMissing<T>(ref component);
        }

        public static T GetComponentCacheFindIfMissing<T>(this GameObject go, ref T component) where T: Component
        {
            if (((T) component) == null)
            {
                component = Object.FindObjectOfType<T>();
            }
            return component;
        }

        public static T GetComponentCacheInChildren<T>(this Component target, ref T component) where T: Component
        {
            T local;
            if (((T) component) == null)
            {
                component = local = target.GetComponentInChildren<T>(true);
            }
            return local;
        }

        public static T GetComponentCacheInChildren<T>(this GameObject go, ref T component) where T: class
        {
            T local;
            if (((T) component) == null)
            {
                component = local = go.GetComponentInChildren<T>(true);
            }
            return local;
        }

        public static T GetComponentCreateIfMissing<T>(this Component target) where T: Component
        {
            return target.get_gameObject().GetComponentCreateIfMissing<T>();
        }

        public static T GetComponentCreateIfMissing<T>(this GameObject go) where T: Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        public static float GetHeight(this RectTransform t)
        {
            return t.get_rect().get_height();
        }

        public static T GetSingletonFindIfMissing<T>(this T target, ref T instance) where T: Component
        {
            if (((T) instance) == null)
            {
                instance = Object.FindObjectOfType<T>();
            }
            return instance;
        }

        public static Vector2 GetSize(this RectTransform t)
        {
            Rect rect = t.get_rect();
            float introduced1 = rect.get_width();
            return new Vector2(introduced1, rect.get_height());
        }

        public static Vector2 GetSizeScaled(this RectTransform t)
        {
            Rect rect = t.get_rect();
            return new Vector2(rect.get_width() * t.get_localScale().x, rect.get_height() * t.get_localScale().y);
        }

        public static TValue GetValueOrGetNullIfMissing<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue: class
        {
            TValue local;
            if (!dictionary.TryGetValue(key, out local))
            {
                return null;
            }
            return local;
        }

        public static TValue GetValueOrSetDefaultIfMissing<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue local;
            if (!dictionary.TryGetValue(key, out local))
            {
                dictionary.Add(key, defaultValue);
                return defaultValue;
            }
            return local;
        }

        public static float GetWith(this RectTransform t)
        {
            return t.get_rect().get_width();
        }

        public static void InitSingletonComponent<T>(this T target, ref T instance) where T: Component
        {
            if ((((T) instance) != null) && (((T) instance) != target))
            {
                object[] objArray1 = new object[] { typeof(T).ToString() };
                Debug.LogErrorFormat("{0} is multiple created", objArray1);
                Object.Destroy(target.get_gameObject());
            }
            else
            {
                instance = target;
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static Rect RectInCanvas(this RectTransform t, Canvas canvas)
        {
            Vector3 vector = t.TransformPoint(t.get_rect().get_center());
            vector = canvas.get_transform().InverseTransformPoint(vector);
            Vector3 sizeScaled = t.GetSizeScaled();
            t.TransformVector(sizeScaled);
            canvas.get_transform().InverseTransformVector(sizeScaled);
            Rect rect2 = new Rect();
            rect2.set_size(sizeScaled);
            rect2.set_center(vector);
            return rect2;
        }

        public static void RemoveComponent<T>(this Component target) where T: Component
        {
            T component = target.get_gameObject().GetComponent<T>();
            if (component != null)
            {
                Object.Destroy(component);
            }
        }

        public static void SafeSendMessage(this GameObject go, string functionName, object obj = null, bool isForceActive = false)
        {
            if (!string.IsNullOrEmpty(functionName))
            {
                if (isForceActive)
                {
                    go.SetActive(true);
                }
                go.SendMessage(functionName, obj, 1);
            }
        }

        public static void Separate(this string str, char separator, bool isFirst, out string str1, out string str2)
        {
            int length = !isFirst ? str.LastIndexOf(separator) : str.IndexOf(separator);
            str1 = str.Substring(0, length);
            str2 = str.Substring(length + 1);
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            Color color = graphic.get_color();
            color.a = alpha;
            graphic.set_color(color);
        }

        public static void SetHeight(this RectTransform t, float height)
        {
            t.SetSizeWithCurrentAnchors(1, height);
        }

        public static void SetSize(this RectTransform t, Vector2 size)
        {
            t.SetWidth(size.x);
            t.SetHeight(size.y);
        }

        public static void SetSize(this RectTransform t, float width, float height)
        {
            t.SetWidth(width);
            t.SetHeight(height);
        }

        public static void SetStretch(this RectTransform t)
        {
            t.set_anchorMin(Vector2.get_zero());
            t.set_anchorMax(Vector2.get_one());
            t.set_sizeDelta(Vector2.get_zero());
        }

        public static void SetWidth(this RectTransform t, float width)
        {
            t.SetSizeWithCurrentAnchors(0, width);
        }

        internal static Rect ToUvRect(this Rect rect, float w, float h)
        {
            return new Rect(rect.get_x() / w, 1f - (rect.get_yMax() / h), rect.get_width() / w, rect.get_height() / h);
        }
    }
}

