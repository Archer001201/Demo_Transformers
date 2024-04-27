using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerAttribute : MonoBehaviour
    {
        public Module currentModule;
        public Module clipboard;
        public List<Module> history;
    }
}
