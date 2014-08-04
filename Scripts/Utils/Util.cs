using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//general helper functions

namespace M8 {
    public struct Util {
        public static int CellToIndex(int row, int col, int numCols) {
            return (row * numCols) + col;
        }

        public static void CellToRowCol(int index, int numCols, out int row, out int col) {
            row = index / numCols;
            col = index % numCols;
        }

        public static int FlagSet(int data, int mask, bool set) {
            return set ? data | mask : data & (~mask);
        }

        public static int FlagSetBit(int data, int bit, bool set) {
            return FlagSet(data, 1<<bit, set);
        }

        public static int FlagFlip(int data, int mask) {
            return data ^ mask;
        }

        public static bool FlagCheck(int data, int mask) {
            return (data & mask) == mask;
        }

        public static bool FlagCheckBit(int data, int bit) {
            return FlagCheck(data, 1<<bit);
        }


        static int CameraCompareDepth(Camera c1, Camera c2) {
            return Mathf.RoundToInt(c1.depth - c2.depth);
        }

        public static Camera[] GetAllCameraDepthSorted() {
            Camera[] cams = Camera.allCameras;
            System.Array.Sort<Camera>(cams, CameraCompareDepth);
            return cams;
        }

        static int ComponentNameCompare<T>(T obj1, T obj2) where T : Component {
            return obj1.name.CompareTo(obj2.name);
        }

        public static GameObject FindGameObjectByNameRecursive(GameObject go, string name) {
            if(go.name == name) {
                return go;
            }
            else {
                //first find it in children
                Transform found = go.transform.Find(name);

                if(found != null) {
                    return found.gameObject;
                }

                //find it inside each child
                foreach(Transform t in go.transform) {
                    GameObject item = FindGameObjectByNameRecursive(t.gameObject, name);

                    if(item != null) {
                        return item;
                    }
                }

                return null;
            }
        }

        public static T[] GetComponentsInChildrenAlphaSort<T>(Component c, bool includeInactive) where T : Component {
            T[] items = c.GetComponentsInChildren<T>(includeInactive);
            System.Array.Sort<T>(items, ComponentNameCompare);

            return items;
        }

        public static T[] GetComponentsInChildrenAlphaSort<T>(GameObject go, bool includeInactive) where T : Component {
            T[] items = go.GetComponentsInChildren<T>(includeInactive);
            System.Array.Sort<T>(items, ComponentNameCompare);

            return items;
        }

        /// <summary>
        /// Get the component from parent of given t, if inclusive is true, try to find the component on t first.
        /// </summary>
        public static T GetComponentUpwards<T>(Transform t, bool inclusive) where T : Component {
            T ret = null;

            Transform parent = inclusive ? t : t.parent;
            while(parent != null) {
                ret = parent.GetComponent<T>();
                if(ret != null)
                    break;

                parent = parent.parent;
            }

            return ret;
        }

        public static bool IsParentOf(Transform child, Transform parent) {
            bool ret = false;

            Transform p = child;
            while(p != null) {
                p = p.parent;
                if(p == parent) {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        public static void SetPhysicsLayerRecursive(Transform t, int layer) {
            t.gameObject.layer = layer;

            foreach(Transform child in t) {
                SetPhysicsLayerRecursive(child, layer);
            }
        }

        /// <summary>
        /// Get the layer the given transform resides in, 0 = root, ie. no parent
        /// </summary>
        public static int GetNodeLayer(Transform t) {
            int ret = 0;

            Transform p = t.parent;
            while(p != null) {
                ret++;
                p = p.parent;
            }

            return ret;
        }

        public static void ShuffleList<T>(List<T> list) {
            for(int i = 0, max = list.Count; i < max; i++) {
                int r = UnityEngine.Random.Range(i, max);
                T obj = list[i];
                T robj = list[r];
                list[i] = robj;
                list[r] = obj;
            }
        }

        public static bool CheckLayerAndTag(GameObject go, int layerMask, string tag) {
            return (layerMask & (1 << go.layer)) != 0 && go.CompareTag(tag);
        }

        public static bool CheckTag(GameObject go, params string[] tags) {
            for(int i = 0; i < tags.Length; i++) {
                if(go.CompareTag(tags[i]))
                    return true;
            }

            return false;
        }

        public static bool CheckTag(Component comp, params string[] tags) {
            for(int i = 0; i < tags.Length; i++) {
                if(comp.CompareTag(tags[i]))
                    return true;
            }

            return false;
        }

        public static ushort MakeWord(byte hb, byte lb) {
            return (ushort)(((hb & 0xff) << 8) | (lb & 0xff));
        }

        public static uint MakeLong(ushort hs, ushort ls) {
            return (uint)(((hs & 0xffff) << 16) | (ls & 0xffff));
        }

        public static uint MakeLong(byte b1, byte b2, byte b3, byte b4) {
            return MakeLong(MakeWord(b1, b2), MakeWord(b3, b4));
        }

        public static ushort GetHiWord(uint i) {
            return (ushort)(i >> 16);
        }

        public static ushort GetLoWord(uint s) {
            return (ushort)(s & 0xffff);
        }

        public static byte GetHiByte(ushort s) {
            return (byte)(s >> 8);
        }

        public static byte GetLoByte(ushort s) {
            return (byte)(s & 0xff);
        }
    }
}