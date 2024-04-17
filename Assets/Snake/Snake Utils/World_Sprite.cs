using System.Linq;
using UnityEngine;

namespace Utils {

    /*
     * Sprite in the World
     * */
    public class World_Sprite {
        
        private const int sortingOrderDefault = 5000;

        public GameObject gameObject;
        public Transform transform;
        private SpriteRenderer spriteRenderer;

        public static World_Sprite Create(Vector3 worldPosition, Vector3 localScale, Sprite sprite, Color color) {
            return new World_Sprite(null, worldPosition, localScale, sprite, color, 0);
        }
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault) {
            return (int)(baseSortingOrder - position.y) + offset;
        }
        public World_Sprite(Transform parent, Vector3 localPosition, Vector3 localScale, Sprite sprite, Color color, int sortingOrderOffset) {
            int sortingOrder = GetSortingOrder(localPosition, sortingOrderOffset);
            gameObject = UtilsClass.CreateWorldSprite(parent, "Sprite", sprite, localPosition, localScale, sortingOrder, color);
            transform = gameObject.transform;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        public void AddBoxCollider2D(bool isTrigger = false, Vector2? size = null, Vector2? offset = null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = isTrigger;

            if (size.HasValue)
            {
                collider.size = size.Value;
            }

            if (offset.HasValue)
            {
                collider.offset = offset.Value;
            }
        }
        public void SetTag(string tag)
        {
                gameObject.tag = tag;
        }

        public void DestroySelf() {
            Object.Destroy(gameObject);
        }

    }
}