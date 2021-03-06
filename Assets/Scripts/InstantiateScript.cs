﻿using System.Collections.Generic;
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

        var instance = Instantiate(go);
        instance.AddComponent<Rigidbody>();
    }

    public void OnInstantiateClick()
    {
        var instantiateOperation = _cubeAssetReference.InstantiateAsync();
        instantiateOperation.Completed += OnInstantiateComplete;
    }

    private void OnInstantiateComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        asyncOperationHandle.Result.AddComponent<Rigidbody>();
        
        Debug.Log($"Instantiate complete with status{asyncOperationHandle.Status} IsDone:{asyncOperationHandle.IsDone}");
    }

    public void OnRemoteLoadClick()
    {
        var remoteAssetTask = Addressables.LoadResourceLocationsAsync(_remoteAssetKey);
        remoteAssetTask.Completed += OnRemoteLoadComplete;
    }

    private async void OnRemoteLoadComplete(AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle)
    {
        foreach (var handler in asyncOperationHandle.Result)
        {
            var instance = await Addressables.InstantiateAsync(handler).Task;
            instance.AddComponent<Rigidbody>();
            
            var transformPosition = instance.transform.position;
            transformPosition.x = Random.Range(-5, 5);
            instance.transform.position = transformPosition;
        }
    }
}
