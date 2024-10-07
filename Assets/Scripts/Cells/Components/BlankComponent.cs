
using UnityEngine;
using Cell;

namespace component
{
    public class BlankComponent : MonoBehaviour
    {
        private Collider2D col;
        private ComponentController controller;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            controller = GetComponentInParent<ComponentController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != "Component") return;
            ComponentType type = collision.gameObject.GetComponent<Drops>().type;
            controller.ContactComponent(this.name, type);
        }
    }
}