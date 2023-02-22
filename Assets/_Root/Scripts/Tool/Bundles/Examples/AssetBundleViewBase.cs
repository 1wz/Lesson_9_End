using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Tool.Bundles.Examples
{
    internal class AssetBundleViewBase : MonoBehaviour
    {
        private const string UrlAssetBundleSprites = "https://drive.google.com/uc?export=download&id=1rQzWdcChHhJJBTe4rf1D0Kwi1a43jxWR";
        private const string UrlAssetBundleAudio = "https://drive.google.com/uc?export=download&id=1I7euU6Hv5yrn1ektprUumbGHEikklk3Y";
        private const string UrlAssetBundleButtonimage = "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/d/rl33YER82ZxFoA";

        [SerializeField] private DataSpriteBundle[] _dataSpriteBundles;
        [SerializeField] private DataAudioBundle[] _dataAudioBundles;
        [SerializeField] private DataSpriteBundle[] _dataButtonImageBundles;

        private AssetBundle _spritesAssetBundle;
        private AssetBundle _audioAssetBundle;
        private AssetBundle _buttonImageAssetBundle;


        protected IEnumerator DownloadAndSetAssetBundles()
        {
            yield return GetSpritesAssetBundle();
            yield return GetAudioAssetBundle();

            if (_spritesAssetBundle != null)
                SetSpriteAssets(_spritesAssetBundle,_dataSpriteBundles);
            else
                Debug.LogError($"AssetBundle {nameof(_spritesAssetBundle)} failed to load");

            if (_audioAssetBundle != null)
                SetAudioAssets(_audioAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_audioAssetBundle)} failed to load");
        }

        protected IEnumerator ChangeButtonImage()
        {
            yield return GetButtonImageAssetBundle();

            if (_buttonImageAssetBundle != null)
                SetSpriteAssets(_buttonImageAssetBundle, _dataButtonImageBundles);
            else
                Debug.LogError($"AssetBundle {nameof(_buttonImageAssetBundle)} failed to load");
        }

        private IEnumerator GetSpritesAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleSprites);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _spritesAssetBundle);
        }

        private IEnumerator GetAudioAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleAudio);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _audioAssetBundle);
        }

        private IEnumerator GetButtonImageAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleButtonimage);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _buttonImageAssetBundle);
        }

        private void StateRequest(UnityWebRequest request, out AssetBundle assetBundle)
        {
            if (request.error == null)
            {
                assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                Debug.Log("Complete");
            }
            else
            {
                assetBundle = null;
                Debug.LogError(request.error);
            }
        }

        private void SetSpriteAssets(AssetBundle assetBundle, DataSpriteBundle[] dataSpriteBundles)
        {
            foreach (DataSpriteBundle data in dataSpriteBundles)
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }

        private void SetAudioAssets(AssetBundle assetBundle)
        {
            foreach (DataAudioBundle data in _dataAudioBundles)
            {
                data.AudioSource.clip = assetBundle.LoadAsset<AudioClip>(data.NameAssetBundle);
                data.AudioSource.Play();
            }
        }
    }
}
