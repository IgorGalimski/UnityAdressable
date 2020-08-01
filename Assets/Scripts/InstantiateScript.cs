using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class InstantiateScript : MonoBehaviour
{
    [SerializeField] 
    private AssetReference _cubeAssetReference;

    [SerializeField] 
    private string _remoteAssetKey;

    public void OnLoadClick()
    {
        var loadAssetOperation = _cubeAssetReference.LoadAssetAsync<GameObject>();
        loadAssetOperation.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        Debug.Log($"Load complete with status{asyncOperationHandle.Status} IsDone:{asyncOperationHandle.IsDone}");
        
        var go = asyncOperationHandle.Result;

        Instantiate(go);
    }

    public void OnInstantiateClick()
    {
        var instantiateOperation = _cubeAssetReference.InstantiateAsync();
        instantiateOperation.Completed += OnInstantiateComplete;
    }

    private void OnInstantiateComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        Debug.Log($"Instantiate complete with status{asyncOperationHandle.Status} IsDone:{asyncOperationHandle.IsDone}");
    }

    public void OnRemoteLoadClick()
    {
        var remoteAssetTask = Addressables.LoadResourceLocationsAsync(_remoteAssetKey);
        remoteAssetTask.Completed += OnRemoteLoadComplete;
    }

    private void OnRemoteLoadComplete(AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle)
    {
        foreach (var handler in asyncOperationHandle.Result)
        {
            Addressables.InstantiateAsync(handler);
        }
    }
}
