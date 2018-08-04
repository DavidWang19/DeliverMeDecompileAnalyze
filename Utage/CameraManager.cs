namespace Utage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UtageExtensions;

    [AddComponentMenu("Utage/Lib/Camera/CameraManager")]
    public class CameraManager : MonoBehaviour, IBinaryIO
    {
        private List<CameraRoot> cameraList;
        private const int Version = 0;

        internal Camera FindCameraByLayer(int layer)
        {
            int num = ((int) 1) << layer;
            foreach (CameraRoot root in this.CameraList)
            {
                Camera cachedCamera = root.LetterBoxCamera.CachedCamera;
                if ((cachedCamera.get_cullingMask() & num) != 0)
                {
                    return cachedCamera;
                }
            }
            return null;
        }

        public CameraRoot FindCameraRoot(string name)
        {
            <FindCameraRoot>c__AnonStorey0 storey = new <FindCameraRoot>c__AnonStorey0 {
                name = name
            };
            return this.CameraList.Find(new Predicate<CameraRoot>(storey.<>m__0));
        }

        internal void OnClear()
        {
            foreach (CameraRoot root in this.CameraList)
            {
                root.OnClear();
            }
        }

        public void OnRead(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            if ((num < 0) || (num > 0))
            {
                object[] args = new object[] { num };
                Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, args));
            }
            else
            {
                int num2 = reader.ReadInt32();
                for (int i = 0; i < num2; i++)
                {
                    string name = reader.ReadString();
                    CameraRoot root = this.FindCameraRoot(name);
                    if (root != null)
                    {
                        reader.ReadBuffer(new Action<BinaryReader>(root.Read));
                    }
                    else
                    {
                        reader.SkipBuffer();
                    }
                }
            }
        }

        public void OnWrite(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(this.CameraList.Count);
            foreach (CameraRoot root in this.CameraList)
            {
                writer.Write(root.get_name());
                writer.WriteBuffer(new Action<BinaryWriter>(root.Write));
            }
        }

        public List<CameraRoot> CameraList
        {
            get
            {
                if (this.cameraList == null)
                {
                    this.cameraList = new List<CameraRoot>(base.GetComponentsInChildren<CameraRoot>(true));
                }
                return this.cameraList;
            }
        }

        public string SaveKey
        {
            get
            {
                return "CameraManager";
            }
        }

        [CompilerGenerated]
        private sealed class <FindCameraRoot>c__AnonStorey0
        {
            internal string name;

            internal bool <>m__0(CameraRoot x)
            {
                return (x.get_name() == this.name);
            }
        }
    }
}

