using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common
{
public class NetworkHelperUI : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private TMP_Text _networkType;
    [Space] 
    [SerializeField] private Button _buttonStartServer;
    [SerializeField] private Button _buttonStartHost;
    [SerializeField] private Button _buttonStartClient;
    [SerializeField] private Button _buttonDisconnect;

    private void UpdateUI()
    {
        _networkType.SetText(_networkManager.IsHost ? "Host" : _networkManager.IsServer ? "Server" : _networkManager.IsClient ? "Client" : "Disconnected");
        
        if (_networkManager.IsServer || _networkManager.IsClient || _networkManager.IsHost)
        {
            _buttonStartServer.gameObject.SetActive(false);
            _buttonStartClient.gameObject.SetActive(false);
            _buttonStartHost.gameObject.SetActive(false);
            _buttonDisconnect.gameObject.SetActive(true);
        }
        else
        {
            _buttonStartServer.gameObject.SetActive(true);
            _buttonStartClient.gameObject.SetActive(true);
            _buttonStartHost.gameObject.SetActive(true);
            _buttonDisconnect.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnTransportFailure += OnTransportFailure;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        
        _buttonStartHost.onClick.AddListener(() => { _networkManager.StartHost(); });
        _buttonStartServer.onClick.AddListener(() => { _networkManager.StartServer(); });
        _buttonStartClient.onClick.AddListener(() => { _networkManager.StartClient(); });
        _buttonDisconnect.onClick.AddListener(() =>
        {
            _networkManager.Shutdown();
            UpdateUI();
        });
        
        InvokeRepeating(nameof(UpdateUI), 0.3f, 0.3f);
        
        UpdateUI();
    }

    private void OnServerStarted()
    {
        Debug.Log("OnServerStarted");
        UpdateUI();
    }

    private void OnTransportFailure()
    {
        Debug.Log("OnTransportFailure");
        UpdateUI();
    }

    private void OnClientConnected(ulong obj)
    {
        Debug.Log($"OnClientConnected {obj}");
        UpdateUI();
    }

    private void OnClientDisconnected(ulong obj)
    {
        Debug.Log($"OnClientDisconnected {obj}");
        UpdateUI();
    }

    private void OnDisable()
    {
        _networkManager.OnServerStarted -= OnServerStarted;
        _networkManager.OnTransportFailure -= OnTransportFailure;
        _networkManager.OnClientConnectedCallback -= OnClientConnected;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
        
        CancelInvoke();
    }
}
}