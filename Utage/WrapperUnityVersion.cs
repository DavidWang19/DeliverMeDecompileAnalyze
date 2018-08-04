namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Profiling;
    using UnityEngine.Rendering;
    using UnityEngine.SceneManagement;

    public class WrapperUnityVersion
    {
        internal static void AddEntryToEventTrigger(EventTrigger eventTrigger, EventTrigger.Entry entry)
        {
            if (eventTrigger.get_triggers() == null)
            {
                eventTrigger.set_triggers(new List<EventTrigger.Entry>());
            }
            eventTrigger.get_triggers().Add(entry);
        }

        public static void CleanCache()
        {
            Caching.ClearCache();
        }

        public static AudioClip CreateAudioClip(string name, int lengthSamples, int channels, int frequency, bool is3D, bool stream)
        {
            return AudioClip.Create(name, lengthSamples, channels, frequency, stream);
        }

        public static Vector2 GetBoxCollider2DOffset(BoxCollider2D col)
        {
            return col.get_offset();
        }

        public static float GetCharacterEndPointX(UguiNovelTextCharacter character)
        {
            return character.Verts[1].position.x;
        }

        public static float GetCharacterInfoWidth(ref CharacterInfo charInfo)
        {
            return (float) charInfo.get_advance();
        }

        public static Rect GetUvRect(ref CharacterInfo info, Texture2D texture)
        {
            if (Mathf.Approximately(info.get_uvTopLeft().x, info.get_uvTopRight().x))
            {
                float num = info.get_uvBottomLeft().x;
                float num2 = info.get_uvTopLeft().x - num;
                float num3 = info.get_uvTopRight().y;
                float num4 = info.get_uvTopLeft().y - num3;
                return new Rect(num * texture.get_width(), num3 * texture.get_height(), num2 * texture.get_width(), num4 * texture.get_height());
            }
            float x = info.get_uvTopLeft().x;
            float num6 = info.get_uvTopRight().x - x;
            float y = info.get_uvTopLeft().y;
            float num8 = info.get_uvBottomLeft().y - y;
            return new Rect(x * texture.get_width(), y * texture.get_height(), num6 * texture.get_width(), num8 * texture.get_height());
        }

        internal static Vector3 GetWorldPositionFromPointerEventData(PointerEventData data)
        {
            return data.get_pointerCurrentRaycast().worldPosition;
        }

        public static bool IsFinishedSplashScreen()
        {
            return SplashScreen.get_isFinished();
        }

        public static bool IsReadyPlayAudioClip(AudioClip clip)
        {
            return (clip.get_loadState() == 2);
        }

        internal static void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        internal static void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public static float MonoHeapMegaSize()
        {
            return (((1f * Profiler.GetMonoHeapSizeLong()) / 1024f) / 1024f);
        }

        public static float MonoUsedMegaSize()
        {
            return (((1f * Profiler.GetMonoUsedSizeLong()) / 1024f) / 1024f);
        }

        public static void SetActivityIndicatorStyle()
        {
        }

        public static void SetBoxCollider2DOffset(BoxCollider2D col, Vector2 offset)
        {
            col.set_offset(offset);
        }

        public static void SetCharacterInfoToVertex(UIVertex[] verts, UguiNovelTextCharacter character, Font font)
        {
            float num5 = 0.1f * character.FontSize;
            float num = character.charInfo.get_minX();
            float num2 = character.charInfo.get_maxX();
            float num3 = character.charInfo.get_minY();
            float num4 = character.charInfo.get_maxY();
            if (!font.get_dynamic())
            {
                num *= character.BmpFontScale;
                num3 *= character.BmpFontScale;
                num2 *= character.BmpFontScale;
                num4 *= character.BmpFontScale;
            }
            Vector2 vector = character.charInfo.get_uvBottomLeft();
            Vector2 vector2 = character.charInfo.get_uvBottomRight();
            Vector2 vector3 = character.charInfo.get_uvTopRight();
            Vector2 vector4 = character.charInfo.get_uvTopLeft();
            verts[0].position.x = verts[3].position.x = num + character.PositionX;
            verts[1].position.x = verts[2].position.x = num2 + character.PositionX;
            verts[0].position.y = verts[1].position.y = (num3 + character.PositionY) + num5;
            verts[2].position.y = verts[3].position.y = (num4 + character.PositionY) + num5;
            verts[0].uv0 = vector;
            verts[1].uv0 = vector2;
            verts[2].uv0 = vector3;
            verts[3].uv0 = vector4;
        }

        public static void SetFontRenderInfo(char c, ref CharacterInfo info, float offsetY, float fontSize, out Vector3 offset, out float width, out float kerningWidth)
        {
            float num = info.get_minX() + ((info.get_maxX() - info.get_minX()) / 2);
            float num2 = ((info.get_maxY() - ((info.get_glyphHeight() + fontSize) / 2f)) + offsetY) + (fontSize / 5f);
            offset = new Vector3(num, num2, 0f);
            width = GetCharacterInfoWidth(ref info);
            kerningWidth = info.get_maxX() - info.get_minX();
        }

        public static void SetNoBackupFlag(string path)
        {
        }

        public static float UsedHeapMegaSize()
        {
            return (((1f * Profiler.get_usedHeapSizeLong()) / 1024f) / 1024f);
        }
    }
}

