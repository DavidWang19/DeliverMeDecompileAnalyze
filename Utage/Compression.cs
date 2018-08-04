namespace Utage
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Compression
    {
        private const int DIC_BITS = 11;
        private const int DIC_MASK = 0x7ff;
        private const int DIC_MASK_HIGH = 0x700;
        private const int DIC_MASK_SHIFTED = 0x70;
        private const int DIC_SIZE = 0x800;
        private const int LENGTH_BITS = 4;
        private const int LENGTH_MASK = 15;
        private const int MAX_LENGTH = 0x12;

        public static byte[] Compress(byte[] bytes)
        {
            int num4;
            int length = bytes.Length;
            byte[] src = BitConverter.GetBytes(length);
            int count = src.Length;
            int num3 = (length + (length / 0x80)) + 1;
            byte[] oData = new byte[num3];
            Compress(oData, out num4, bytes);
            byte[] dst = new byte[num4 + 4];
            Buffer.BlockCopy(src, 0, dst, 0, count);
            Buffer.BlockCopy(oData, 0, dst, count, num4);
            return dst;
        }

        private static void Compress(byte[] oData, out int oSize, byte[] iData)
        {
            int length = iData.Length;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            Index index = new Index();
            while (num3 < length)
            {
                Node node;
                int num5 = 0;
                int num6 = 0;
                int num7 = min(0x12, length - num3);
                for (int i = index.getFirst(iData[num3]); !index.isEnd(i); i = node.mNext)
                {
                    node = index.getNode(i);
                    int mPos = node.mPos;
                    int num10 = 1;
                    while (num10 < num7)
                    {
                        if (iData[mPos + num10] != iData[num3 + num10])
                        {
                            break;
                        }
                        num10++;
                    }
                    if (num5 < num10)
                    {
                        num6 = mPos;
                        num5 = num10;
                        if (num5 == num7)
                        {
                            break;
                        }
                    }
                }
                if (num5 >= 3)
                {
                    for (int j = 0; j < num5; j++)
                    {
                        int pos = (num3 + j) - 0x800;
                        if (pos >= 0)
                        {
                            index.remove(iData[pos], pos);
                        }
                        index.add(iData[num3 + j], num3 + j);
                    }
                    if (num4 < num3)
                    {
                        oData[num2] = (byte) ((num3 - num4) - 1);
                        num2++;
                        for (int k = num4; k < num3; k++)
                        {
                            oData[num2] = iData[k];
                            num2++;
                        }
                    }
                    int num14 = num5 - 3;
                    int num15 = (num3 - num6) - 1;
                    int num16 = 0x80 | num14;
                    num16 |= (num15 & 0x700) >> 4;
                    oData[num2] = (byte) num16;
                    oData[num2 + 1] = (byte) (num15 & 0xff);
                    num2 += 2;
                    num3 += num5;
                    num4 = num3;
                }
                else
                {
                    int num17 = num3 - 0x800;
                    if (num17 >= 0)
                    {
                        index.remove(iData[num17], num17);
                    }
                    index.add(iData[num3], num3);
                    num3++;
                    if ((num3 - num4) == 0x80)
                    {
                        oData[num2] = (byte) ((num3 - num4) - 1);
                        num2++;
                        for (int m = num4; m < num3; m++)
                        {
                            oData[num2] = iData[m];
                            num2++;
                        }
                        num4 = num3;
                    }
                }
            }
            if (num4 < num3)
            {
                oData[num2] = (byte) ((num3 - num4) - 1);
                num2++;
                for (int n = num4; n < num3; n++)
                {
                    oData[num2] = iData[n];
                    num2++;
                }
            }
            oSize = num2;
        }

        public static byte[] Decompress(byte[] bytes)
        {
            int oSize = BitConverter.ToInt32(bytes, 0);
            byte[] oData = new byte[oSize];
            Decompress(oData, out oSize, bytes);
            return oData;
        }

        private static void Decompress(byte[] oData, out int oSize, byte[] iData)
        {
            int num = 0;
            int length = iData.Length;
            for (int i = 4; i < length; i++)
            {
                int num4;
                if ((iData[i] & 0x80) != 0)
                {
                    num4 = iData[i] & 15;
                    num4 += 3;
                    int num5 = ((iData[i] & 0x70) << 4) | iData[i + 1];
                    num5++;
                    for (int j = 0; j < num4; j++)
                    {
                        oData[num + j] = oData[(num - num5) + j];
                    }
                    i++;
                }
                else
                {
                    num4 = iData[i] + 1;
                    for (int k = 0; k < num4; k++)
                    {
                        oData[num + k] = iData[(i + 1) + k];
                    }
                    i += num4;
                }
                num += num4;
            }
            oSize = num;
        }

        private static int max(int a, int b)
        {
            return ((a <= b) ? b : a);
        }

        private static int min(int a, int b)
        {
            return ((a >= b) ? b : a);
        }

        private class Index
        {
            private Compression.Node[] mNodes = new Compression.Node[0x900];
            private int[] mStack = new int[0x800];
            private int mStackPos;

            public Index()
            {
                for (int i = 0; i < 0x900; i++)
                {
                    this.mNodes[i] = new Compression.Node();
                }
                for (int j = 0x800; j < 0x900; j++)
                {
                    this.mNodes[j].mNext = this.mNodes[j].mPrev = j;
                }
                for (int k = 0; k < 0x800; k++)
                {
                    this.mStack[k] = k;
                }
                this.mStackPos = 0x800;
            }

            public void add(byte c, int pos)
            {
                this.mStackPos--;
                int index = this.mStack[this.mStackPos];
                Compression.Node node = this.mNodes[index];
                Compression.Node node2 = this.mNodes[0x800 + c];
                node.mNext = node2.mNext;
                node.mPrev = 0x800 + c;
                node.mPos = pos;
                this.mNodes[node2.mNext].mPrev = index;
                node2.mNext = index;
            }

            public int getFirst(byte c)
            {
                return this.mNodes[0x800 + c].mNext;
            }

            public Compression.Node getNode(int i)
            {
                return this.mNodes[i];
            }

            public bool isEnd(int idx)
            {
                return (idx >= 0x800);
            }

            public void remove(byte c, int pos)
            {
                int mPrev = this.mNodes[0x800 + c].mPrev;
                Compression.Node node = this.mNodes[mPrev];
                if (node.mPos != pos)
                {
                    Debug.LogError("n.mPos != pos");
                }
                this.mStack[this.mStackPos] = this.mNodes[node.mPrev].mNext;
                this.mStackPos++;
                this.mNodes[node.mPrev].mNext = node.mNext;
                this.mNodes[node.mNext].mPrev = node.mPrev;
            }
        }

        private class Node
        {
            public int mNext;
            public int mPos;
            public int mPrev;
        }
    }
}

