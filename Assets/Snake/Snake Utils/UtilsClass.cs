using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utils {

    /*
     * Various assorted utilities functions
     * */
    public static class UtilsClass {
        
        private static readonly Vector3 Vector3zero = Vector3.zero;
        private static readonly Vector3 Vector3one = Vector3.one;
        private static readonly Vector3 Vector3yDown = new Vector3(0,-1);

        private const int sortingOrderDefault = 5000;
        
        // Get Main Canvas Transform
        private static Transform cachedCanvasTransform;
        public static Transform GetCanvasTransform() {
            if (cachedCanvasTransform == null) {
                Canvas canvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (canvas != null) {
                    cachedCanvasTransform = canvas.transform;
                }
            }
            return cachedCanvasTransform;
        }

        // Get Default Unity Font, used in text objects if no font given
        public static Font GetDefaultFont() {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }      
        // Create a Sprite in the World
        public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition, Vector3 localScale, int sortingOrder, Color color) {
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return gameObject;
        }

        // Parse a int, return default if failed
	    public static int Parse_Int(string txt, int _default) {
		    int i;
		    if (!int.TryParse(txt, out i)) {
			    i = _default;
		    }
		    return i;
	    }
	    public static int Parse_Int(string txt) {
            return Parse_Int(txt, -1);
	    }



        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPositionZeroZ() {
            Vector3 vec = GetMouseWorldPosition(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPosition() {
            return GetMouseWorldPosition(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPosition(Camera worldCamera) {
            return GetMouseWorldPosition(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        
   
	    public static string Dec_to_Hex(int value) {
		    //Returns 00-FF
		    return value.ToString("X2");
	    }
	    public static int Hex_to_Dec(string hex) {
		    //Returns 0-255
		    return Convert.ToInt32(hex, 16);
	    }
	    public static string Dec01_to_Hex(float value) {
		    //Returns a hex string based on a number between 0->1
		    return Dec_to_Hex((int)Mathf.Round(value*255f));
	    }
	    public static float Hex_to_Dec01(string hex) {
		    //Returns a float between 0->1
		    return Hex_to_Dec(hex)/255f;
	    }
    }

}