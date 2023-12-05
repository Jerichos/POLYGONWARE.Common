using POLYGONWARE.Common.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace POLYGONWARE.Common
{

public class DeveloperConsole : Singleton<DeveloperConsole>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadDeveloperConsole()
    {
        Addressables.InstantiateAsync("Assets/POLYGONWARE.Common/Prefabs/DeveloperConsole.prefab").Completed += handle =>
        {
            if (!handle.Result)
            {
                Debug.LogError("Prefab could not be loaded.");
            }
        };
    }
}
}