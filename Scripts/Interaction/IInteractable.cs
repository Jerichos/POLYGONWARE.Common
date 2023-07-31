using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.Interaction
{
public interface IInteractable
{
    void Interact(Object who, GenericDelegate<InteractionResult> callback);
    
    public struct InteractionResult
    {
        public Component Component;
        public InteractionResultCode ResultCode;
    }

    public enum InteractionResultCode
    {
        FAILED,
        SUCCESS
    }
}
}