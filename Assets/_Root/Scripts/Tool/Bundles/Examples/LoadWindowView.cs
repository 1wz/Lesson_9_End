using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Tool.Bundles.Examples
{
    internal class LoadWindowView : AssetBundleViewBase
    {
        [Header("Asset Bundles")]
        [SerializeField] private Button _loadAssetsButton;

        [Header("Addressables")]
        [SerializeField] private AssetReference _spawningButtonPrefab;
        [SerializeField] private RectTransform _spawnedButtonsContainer;
        [SerializeField] private Button _spawnAssetButton;

        [Header("Homework")]
        [SerializeField] private Button _changeInteractableButton;
        [SerializeField] private Button _addBackgroundButton;
        [SerializeField] private Button _removeBackgroundButton;
        [SerializeField] private AssetReference _backgroundPrefab;
        [SerializeField] private RectTransform _UIRectTransform;

        private readonly List<AsyncOperationHandle<GameObject>> _addressablePrefabs =
            new List<AsyncOperationHandle<GameObject>>();
        private GameObject _backGround;


        private void Start()
        {
            _loadAssetsButton.onClick.AddListener(LoadAssets);
            _spawnAssetButton.onClick.AddListener(SpawnPrefab);
            _changeInteractableButton.onClick.AddListener(ChangeButton);
            _addBackgroundButton.onClick.AddListener(AddBackground);
            _removeBackgroundButton.onClick.AddListener(RemoveBackground);
        }

        private void OnDestroy()
        {
            _loadAssetsButton.onClick.RemoveAllListeners();
            _spawnAssetButton.onClick.RemoveAllListeners();
            _changeInteractableButton.onClick.RemoveAllListeners();
            _addBackgroundButton.onClick.RemoveAllListeners();
            _removeBackgroundButton.onClick.RemoveAllListeners();

            DespawnPrefabs();
        }


        private void LoadAssets()
        {
            _loadAssetsButton.interactable = false;
            StartCoroutine(DownloadAndSetAssetBundles());
        }

        private void ChangeButton()
        {
            _changeInteractableButton.interactable = false;
            StartCoroutine(ChangeButtonImage());
        }

        private void SpawnPrefab()
        {
            AsyncOperationHandle<GameObject> addressablePrefab =
                Addressables.InstantiateAsync(_spawningButtonPrefab, _spawnedButtonsContainer);

            _addressablePrefabs.Add(addressablePrefab);
        }

        private void DespawnPrefabs()
        {
            foreach (AsyncOperationHandle<GameObject> addressablePrefab in _addressablePrefabs)
                Addressables.ReleaseInstance(addressablePrefab);

            _addressablePrefabs.Clear();
        }

        private void AddBackground()
        {
            if(_backGround==null)
            {
                AsyncOperationHandle<GameObject> backGround =
                    Addressables.InstantiateAsync(_backgroundPrefab, _UIRectTransform);
                backGround.WaitForCompletion();
                _backGround = backGround.Result;
                _backGround.transform.SetAsFirstSibling();
            }
        }

        private void RemoveBackground()
        {
            if(_backGround!=null)
            {
                Destroy(_backGround);
                _backGround = null;
            }
        }
    }
}
