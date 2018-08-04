namespace Utage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UguiNovelTextEmojiData : ScriptableObject
    {
        [SerializeField]
        private int size;
        private Dictionary<char, Sprite> spriteDictionary;
        private Dictionary<string, Sprite> spriteDictionaryStringKey;
        [SerializeField]
        private List<Sprite> spriteTbl;

        public bool Contains(char c)
        {
            return this.SpriteDictionary.ContainsKey(c);
        }

        public Sprite GetSprite(char c)
        {
            Sprite sprite;
            if (this.SpriteDictionary.TryGetValue(c, out sprite))
            {
                return sprite;
            }
            return null;
        }

        public Sprite GetSprite(string key)
        {
            Sprite sprite;
            if (this.SpriteDictionaryStringKey.TryGetValue(key, out sprite))
            {
                return sprite;
            }
            return null;
        }

        private void MakeDictionary()
        {
            this.spriteDictionary = new Dictionary<char, Sprite>();
            this.spriteDictionaryStringKey = new Dictionary<string, Sprite>();
            foreach (Sprite sprite in this.spriteTbl)
            {
                if (sprite != null)
                {
                    this.spriteDictionaryStringKey.Add(sprite.get_name(), sprite);
                    try
                    {
                        char key = Convert.ToChar(Convert.ToInt32(sprite.get_name(), 0x10));
                        this.spriteDictionary.Add(key, sprite);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public int Size
        {
            get
            {
                if (this.size == 0)
                {
                    Debug.LogError("EmojiData size is zero", this);
                    return 8;
                }
                return this.size;
            }
        }

        private Dictionary<char, Sprite> SpriteDictionary
        {
            get
            {
                if (this.spriteDictionary == null)
                {
                    this.MakeDictionary();
                }
                return this.spriteDictionary;
            }
        }

        private Dictionary<string, Sprite> SpriteDictionaryStringKey
        {
            get
            {
                if (this.spriteDictionaryStringKey == null)
                {
                    this.MakeDictionary();
                }
                return this.spriteDictionaryStringKey;
            }
        }
    }
}

