
using UnityEngine;
using Cell;

namespace component
{
    public class Drops : BaseComponent
    {
        public ComponentType type;
        Collider2D col;
        private void Awake()
        {
            col = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != "Cell") return;
            col.enabled = false;
            Destroy(this.gameObject);
        }
    }
}