using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class TargetUI : MonoBehaviour
    {
        private GameObject m_target = null;
        public GameObject Target
        {
            get => m_target;
            set => m_target = value;
        }
    }
}

