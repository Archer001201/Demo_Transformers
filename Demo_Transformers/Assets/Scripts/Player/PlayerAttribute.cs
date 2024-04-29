using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerAttribute : MonoBehaviour
    {
        public int health = 3;
        public int energy = 3;
        public Module currentModule;
        public Module clipboard;
        public List<Module> history;
        // public int historyAmount = 1;
    }
}
