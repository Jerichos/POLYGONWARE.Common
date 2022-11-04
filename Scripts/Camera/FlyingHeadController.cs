using UnityEngine;

namespace POLYGONWARE.Common
{
    public class FlyingHeadController : Controller
    {
        protected override void HandleControl(IControllable controllable)
        {
            if (controllable is FlyingHead flyingHead)
            {
                
            }
        }
    }
}