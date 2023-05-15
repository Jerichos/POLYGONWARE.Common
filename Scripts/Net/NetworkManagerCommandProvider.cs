using Unity.Netcode;
using UnityEngine;

namespace POLYGONWARE.Common
{
public class NetworkManagerCommandProvider : CommandProviderBehaviour
{
    [SerializeField] private NetworkManager _networkManager;

    protected override void RegisterCommands()
    {
        AddCommand("server", "start", "stop");
        AddCommand("host", "start", "stop");
        AddCommand("client", "start", "stop");
    }

    protected override void OnCommandExecute(string command, params object[] arguments)
    {
        switch (command)
        {
            case "server":
                switch (arguments[0])
                {
                    case "start":
                        _networkManager.StartServer();
                        break;
                    case "stop":
                        _networkManager.Shutdown();
                        break;
                }
                break;
            case "host":
                switch (arguments[0])
                {
                    case "start":
                        _networkManager.StartHost();
                        break;
                    case "stop":
                        _networkManager.Shutdown();
                        break;
                }
                break;
            case "client":
                switch (arguments[0])
                {
                    case "start":
                        _networkManager.StartClient();
                        break;
                    case "stop":
                        _networkManager.Shutdown();
                        break;
                }
                break;
            default: Debug.LogWarning($"no such a command: {command}");
                break;
        }
    }
}
}