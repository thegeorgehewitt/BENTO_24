using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    [SerializeField] string _adUnitId = null; // This will remain null for unsupported platforms

    [SerializeField] private bool rewarded = false;
    private static RewardedAdsButton instance;

    void Awake()
    {
        rewarded = false;

        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        // prevent multiple instances from existing
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        foreach (Button button in FindObjectsOfType<Button>())
        {
            if (button.CompareTag("RewardSystem"))
            {
                _showAdButton = button;
                break;
            }
        }       

        if (_showAdButton != null)
        {
            // Disable the button until the ad is ready to show:
            _showAdButton.interactable = false;
            _showAdButton.gameObject.SetActive(false);

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        foreach (Button button in FindObjectsOfType<Button>())
        {
            if (button.CompareTag("RewardSystem"))
            {
                _showAdButton = button;
                break;
            }
        }

        if (_showAdButton != null)
        {
            // Disable the button until the ad is ready to show:
            _showAdButton.interactable = false;
            _showAdButton.gameObject.SetActive(false);
        }

        rewarded = false;

        LoadAd();
    }

    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId) && _showAdButton)
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
            _showAdButton.gameObject.SetActive(true);
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        //Debug.Log("ShowAd called and button made uninteractable");
        if (_showAdButton)
        {
            // Disable the button:
            _showAdButton.interactable = false;
            _showAdButton.gameObject.SetActive(false);
        }
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            //Debug.Log("Unity Ads Rewarded Ad Completed");

            if(MainManager.Instance != null && rewarded == false)
            {
                rewarded = true;
                MainManager.Instance.ChangeFunds(5);
            }
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        //Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        if (_showAdButton)
        {
            //Debug.Log("unsubcribed");
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }
    }
}