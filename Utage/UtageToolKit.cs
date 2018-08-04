namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UtageExtensions;

    public class UtageToolKit
    {
        private static CultureInfo cultureInfJp = new CultureInfo("ja-JP");

        [Obsolete]
        internal static T[] AddArrayUnique<T>(T[] array, T[] addArray)
        {
            List<T> list = new List<T>(array);
            foreach (T local in addArray)
            {
                if (!list.Contains(local))
                {
                    list.Add(local);
                }
            }
            return list.ToArray();
        }

        [Obsolete]
        public static GameObject AddChild(Transform parent, GameObject go)
        {
            return parent.AddChild(go, Vector3.get_zero(), Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChild(Transform parent, GameObject go, Vector3 localPosition)
        {
            return parent.AddChild(go, localPosition, Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChild(Transform parent, GameObject go, Vector3 localPosition, Vector3 localScale)
        {
            return parent.AddChild(go, localPosition, localScale);
        }

        [Obsolete]
        public static GameObject AddChildGameObject(Transform parent, string name)
        {
            return parent.AddChildGameObject(name, Vector3.get_zero(), Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChildGameObject(Transform parent, string name, Vector3 localPosition)
        {
            return parent.AddChildGameObject(name, localPosition, Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChildGameObject(Transform parent, string name, Vector3 localPosition, Vector3 localScale)
        {
            return parent.AddChildGameObject(name, localPosition, localScale);
        }

        [Obsolete]
        public static T AddChildGameObjectComponent<T>(Transform parent, string name) where T: Component
        {
            return parent.AddChildGameObjectComponent<T>(name, Vector3.get_zero(), Vector3.get_one());
        }

        [Obsolete]
        public static T AddChildGameObjectComponent<T>(Transform parent, string name, Vector3 localPosition) where T: Component
        {
            return parent.AddChildGameObjectComponent<T>(name, localPosition, Vector3.get_one());
        }

        [Obsolete]
        public static T AddChildGameObjectComponent<T>(Transform parent, string name, Vector3 localPosition, Vector3 localScale) where T: Component
        {
            return parent.AddChildGameObjectComponent<T>(name, localPosition, localScale);
        }

        [Obsolete]
        public static GameObject AddChildPrefab(Transform parent, GameObject prefab)
        {
            return parent.AddChildPrefab(prefab, Vector3.get_zero(), Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChildPrefab(Transform parent, GameObject prefab, Vector3 localPosition)
        {
            return parent.AddChildPrefab(prefab, localPosition, Vector3.get_one());
        }

        [Obsolete]
        public static GameObject AddChildPrefab(Transform parent, GameObject prefab, Vector3 localPosition, Vector3 localScale)
        {
            return parent.AddChildPrefab(prefab, localPosition, localScale);
        }

        [Obsolete]
        public static GameObject AddChildUnityObject(Transform parent, Object obj)
        {
            return parent.AddChildUnityObject(obj);
        }

        [Obsolete]
        public static void AddEventTriggerEntry(EventTrigger eventTrigger, UnityAction<BaseEventData> action, EventTriggerType eventTriggerType)
        {
            <AddEventTriggerEntry>c__AnonStorey0 storey = new <AddEventTriggerEntry>c__AnonStorey0 {
                action = action
            };
            EventTrigger.Entry entry = new EventTrigger.Entry();
            EventTrigger.TriggerEvent event2 = new EventTrigger.TriggerEvent();
            event2.AddListener(new UnityAction<BaseEventData>(storey, (IntPtr) this.<>m__0));
            entry.callback = event2;
            entry.eventID = eventTriggerType;
            WrapperUnityVersion.AddEntryToEventTrigger(eventTrigger, entry);
        }

        public static Texture2D CaptureScreen()
        {
            return CaptureScreen(new Rect(0f, 0f, (float) Screen.get_width(), (float) Screen.get_height()));
        }

        public static Texture2D CaptureScreen(Rect rect)
        {
            return CaptureScreen(3, rect);
        }

        public static Texture2D CaptureScreen(TextureFormat format, Rect rect)
        {
            Texture2D textured = new Texture2D((int) rect.get_width(), (int) rect.get_height(), format, false);
            try
            {
                textured.ReadPixels(rect, 0, 0);
                textured.Apply();
            }
            catch
            {
            }
            return textured;
        }

        [Obsolete]
        public static void ChangeLayerAllChildren(Transform trans, int layer)
        {
            trans.get_gameObject().ChangeLayerDeep(layer);
        }

        public static Texture2D CreateResizeTexture(Texture2D tex, int width, int height)
        {
            if (tex == null)
            {
                return null;
            }
            return CreateResizeTexture(tex, width, height, tex.get_format(), tex.get_mipmapCount() > 1);
        }

        public static Texture2D CreateResizeTexture(Texture2D tex, int width, int height, TextureFormat format)
        {
            return CreateResizeTexture(tex, width, height, format, false);
        }

        public static Texture2D CreateResizeTexture(Texture2D tex, int width, int height, TextureFormat format, bool isMipmap)
        {
            if (tex == null)
            {
                return null;
            }
            TextureWrapMode mode = tex.get_wrapMode();
            tex.set_wrapMode(1);
            Color[] colorArray = new Color[width * height];
            int index = 0;
            for (int i = 0; i < height; i++)
            {
                float num3 = (1f * i) / ((float) (height - 1));
                for (int j = 0; j < width; j++)
                {
                    float num5 = (1f * j) / ((float) (width - 1));
                    colorArray[index] = tex.GetPixelBilinear(num5, num3);
                    index++;
                }
            }
            tex.set_wrapMode(mode);
            Texture2D textured = new Texture2D(width, height, format, isMipmap);
            textured.SetPixels(colorArray);
            textured.Apply();
            return textured;
        }

        public static Sprite CreateSprite(Texture2D tex, float pixelsToUnits)
        {
            return CreateSprite(tex, pixelsToUnits, new Vector2(0.5f, 0.5f));
        }

        public static Sprite CreateSprite(Texture2D tex, float pixelsToUnits, Vector2 pivot)
        {
            if (tex == null)
            {
                Debug.LogError("texture is null");
                tex = Texture2D.get_whiteTexture();
            }
            if (tex.get_mipmapCount() > 1)
            {
                object[] args = new object[] { tex.get_name() };
                Debug.LogWarning(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.SpriteMimMap, args));
            }
            Rect rect = new Rect(0f, 0f, (float) tex.get_width(), (float) tex.get_height());
            return Sprite.Create(tex, rect, pivot, pixelsToUnits);
        }

        public static string DateToStringJp(DateTime date)
        {
            return date.ToString(cultureInfJp);
        }

        [Obsolete]
        public static void DestroyChildren(Transform parent)
        {
            parent.DestroyChildren();
        }

        [Obsolete]
        public static void DestroyChildrenInEditorOrPlayer(Transform parent)
        {
            parent.DestroyChildrenInEditorOrPlayer();
        }

        [Obsolete]
        public static Transform FindInChirdlen(Transform trasnform, string name)
        {
            return trasnform.FindDeep(name, true);
        }

        [Obsolete]
        public static T FindParentComponent<T>(Transform transform) where T: Component
        {
            return transform.GetComponentInParent<T>();
        }

        [Obsolete]
        public static T GetCompoentInChildrenCreateIfMissing<T>(Transform trasnform) where T: Component
        {
            return ((Component) trasnform).GetComponentCreateIfMissing<T>();
        }

        [Obsolete]
        internal static T GetComponentCreateIfMissing<T>(GameObject go) where T: Component
        {
            return go.GetComponentCreateIfMissing<T>();
        }

        [Obsolete]
        public static T GetInterfaceCompoent<T>(GameObject go) where T: class
        {
            return go.GetComponent<T>();
        }

        [Obsolete]
        public static T[] GetInterfaceCompoents<T>(GameObject go) where T: class
        {
            return go.GetComponents<T>();
        }

        public static bool IsHankaku(char c)
        {
            if (((c > '~') && (c != '\x00a5')) && ((c != '‾') && ((c < 0xff61) || (c > 0xff9f))))
            {
                return false;
            }
            return true;
        }

        public static bool IsPlatformStandAlone()
        {
            RuntimePlatform platform = Application.get_platform();
            if (((platform != 1) && (platform != 2)) && (platform != 13))
            {
                return false;
            }
            return true;
        }

        public static bool IsPlatformStandAloneOrEditor()
        {
            return (Application.get_isEditor() || IsPlatformStandAlone());
        }

        [Obsolete]
        public static Color ReadColor(BinaryReader reader)
        {
            Color color;
            color.r = reader.ReadSingle();
            color.g = reader.ReadSingle();
            color.b = reader.ReadSingle();
            color.a = reader.ReadSingle();
            return color;
        }

        [Obsolete]
        public static void ReadLocalTransform(Transform transform, BinaryReader reader)
        {
            Vector3 pos = new Vector3();
            Vector3 euler = new Vector3();
            Vector3 scale = new Vector3();
            ReadLocalTransform(reader, out pos, out euler, out scale);
            transform.set_localPosition(pos);
            transform.set_localEulerAngles(euler);
            transform.set_localScale(scale);
        }

        [Obsolete]
        public static void ReadLocalTransform(BinaryReader reader, out Vector3 pos, out Vector3 euler, out Vector3 scale)
        {
            pos.x = reader.ReadSingle();
            pos.y = reader.ReadSingle();
            pos.z = reader.ReadSingle();
            euler.x = reader.ReadSingle();
            euler.y = reader.ReadSingle();
            euler.z = reader.ReadSingle();
            scale.x = reader.ReadSingle();
            scale.y = reader.ReadSingle();
            scale.z = reader.ReadSingle();
        }

        [Obsolete]
        public static void SafeSendMessage(object obj, GameObject target, string functionName, bool isForceActive = false)
        {
            SafeSendMessage(target, functionName, obj, isForceActive);
        }

        [Obsolete]
        public static void SafeSendMessage(GameObject target, string functionName, object obj = null, bool isForceActive = false)
        {
            if (target != null)
            {
                target.SafeSendMessage(functionName, obj, isForceActive);
            }
        }

        [Obsolete]
        public static bool TryParaseEnum<T>(string str, out T val)
        {
            try
            {
                val = (T) Enum.Parse(typeof(T), str);
                return true;
            }
            catch (Exception)
            {
                val = default(T);
                return false;
            }
        }

        [Obsolete]
        public static void WriteColor(Color color, BinaryWriter writer)
        {
            writer.Write(color.r);
            writer.Write(color.g);
            writer.Write(color.b);
            writer.Write(color.a);
        }

        [Obsolete]
        public static void WriteLocalTransform(Transform transform, BinaryWriter writer)
        {
            writer.Write(transform.get_localPosition().x);
            writer.Write(transform.get_localPosition().y);
            writer.Write(transform.get_localPosition().z);
            writer.Write(transform.get_localEulerAngles().x);
            writer.Write(transform.get_localEulerAngles().y);
            writer.Write(transform.get_localEulerAngles().z);
            writer.Write(transform.get_localScale().x);
            writer.Write(transform.get_localScale().y);
            writer.Write(transform.get_localScale().z);
        }

        [CompilerGenerated]
        private sealed class <AddEventTriggerEntry>c__AnonStorey0
        {
            internal UnityAction<BaseEventData> action;

            internal void <>m__0(BaseEventData eventData)
            {
                this.action.Invoke(eventData);
            }
        }
    }
}

