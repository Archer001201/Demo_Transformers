using UnityEngine;
using Utilities;

namespace Player
{
    public class ThrusterModule : BaseModule
    {
        protected override void Awake()
        {
            base.Awake();
            module = Module.Thruster;
        }
    }
}
