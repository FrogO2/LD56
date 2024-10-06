using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace component
{
    public class ComponentController : MonoBehaviour
    {
        readonly List<Vector3> posList = new List<Vector3> { new Vector3(-0.0387f, 1.3220f, 0), new Vector3(1.1279f, 0.7013f, 0), new Vector3(1.1221f, -0.6205f, 0),
                                                    new Vector3(0.0187f, -1.2814f, 0), new Vector3(-1.1192f, -0.6320f, 0), new Vector3(-1.1594f, 0.6093f, 0)};
        public List<BaseComponent> ComponentList;

        public void ChangeComponent()
        {

        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
