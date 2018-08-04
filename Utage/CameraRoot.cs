namespace Utage
{
    using System;
    using System.IO;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/Camera/CameraRoot")]
    public class CameraRoot : MonoBehaviour
    {
        private Utage.LetterBoxCamera letterBoxCamera;
        private Vector3 startEulerAngles = Vector3.get_one();
        private Vector3 startPosition = Vector3.get_zero();
        private Vector3 startScale = Vector3.get_one();
        private const int Version = 0;

        private void Awake()
        {
            this.startPosition = base.get_transform().get_localPosition();
            this.startScale = base.get_transform().get_localScale();
            this.startEulerAngles = base.get_transform().get_localEulerAngles();
        }

        internal void OnClear()
        {
            base.get_transform().set_localPosition(this.startPosition);
            base.get_transform().set_localScale(this.startScale);
            base.get_transform().set_localEulerAngles(this.startEulerAngles);
            foreach (ImageEffectBase base2 in this.LetterBoxCamera.GetComponents<ImageEffectBase>())
            {
                Object.Destroy(base2);
            }
        }

        public void Read(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                reader.ReadLocalTransform(base.get_transform());
                reader.ReadBuffer(new Action<BinaryReader>(this.LetterBoxCamera.Read));
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    ImageEffectBase base2;
                    bool flag;
                    string type = reader.ReadString();
                    if (!ImageEffectUtil.TryGetComonentCreateIfMissing(type, out base2, out flag, this.LetterBoxCamera.get_gameObject()))
                    {
                        Debug.LogError("Unkonwo Image Effect Type [ " + type + " ]");
                        reader.SkipBuffer();
                    }
                    else
                    {
                        reader.ReadBuffer(new Action<BinaryReader>(base2.Read));
                    }
                }
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(0);
            writer.WriteLocalTransform(base.get_transform());
            writer.WriteBuffer(new Action<BinaryWriter>(this.LetterBoxCamera.Write));
            ImageEffectBase[] components = this.LetterBoxCamera.GetComponents<ImageEffectBase>();
            writer.Write(components.Length);
            for (int i = 0; i < components.Length; i++)
            {
                string str = ImageEffectUtil.ToImageEffectType(components[i].GetType());
                writer.Write(str);
                writer.WriteBuffer(new Action<BinaryWriter>(components[i].Write));
            }
        }

        public Utage.LetterBoxCamera LetterBoxCamera
        {
            get
            {
                if (this.letterBoxCamera == null)
                {
                    this.letterBoxCamera = base.get_gameObject().GetComponentInChildren<Utage.LetterBoxCamera>(true);
                }
                return this.letterBoxCamera;
            }
        }
    }
}

