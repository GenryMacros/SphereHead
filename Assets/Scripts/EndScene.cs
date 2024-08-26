using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EndScene : MonoBehaviour
{
    [SerializeField] 
    private OpeningDoor _door;

    [SerializeField] 
    private IntensifiedLight _light;

    [SerializeField] 
    private float _blackScreenTime;
    [SerializeField] 
    private float _noiseChangeTime;
    [SerializeField] 
    private float _maxNoise;
    [SerializeField] 
    private RawImage _blackImage;
    [SerializeField] 
    private VideoPlayer _videoPlayer;
    
    [SerializeField] 
    private Material _noiseMaterial;
    public AudioSource noiseSource;
    public VideoPlayer backGround;
    
    private bool _isDoorOpen = false;
    private bool _isLightOn = false;
    private float _timeSinceDoorOpen;
    private bool _isAbleToQuit = false;
    
    void Start()
    {
        _door.activateCallback += DoorOpened;
        _light.activateCallback += LightActive;
        _noiseMaterial.SetFloat("_noise_amount", 0.0f);
        backGround.url = System.IO.Path.Combine (Application.streamingAssetsPath,"jumpscare_final.mp4"); 
    }
    
    void Update()
    {
        if(_videoPlayer.frame + 1 >= (long)_videoPlayer.frameCount && _isAbleToQuit)
        {
            Application.Quit();
            Debug.Log("Quit");
        }

        if (_isDoorOpen)
        {
            _timeSinceDoorOpen += Time.deltaTime;
            if (_timeSinceDoorOpen < _noiseChangeTime)
            {
                float t = _timeSinceDoorOpen / _noiseChangeTime;
                _noiseMaterial.SetFloat("_noise_amount", Mathf.Lerp(0.0f, _maxNoise, t));
            }
            
            if (_timeSinceDoorOpen >= _noiseChangeTime)
            {
                _isDoorOpen = false;
                StartPrepareJumpscare();
            }
        }
    }
    
    void DoorOpened()
    {
        _isDoorOpen = true;
        noiseSource.Play();
    }

    void LightActive()
    {
        _isLightOn = true;
    }

    void StartPrepareJumpscare()
    {
        _blackImage.gameObject.SetActive(true);
        Invoke(nameof(Jumpscare), _blackScreenTime);
    }

    void Jumpscare()
    {
        _videoPlayer.Play();
        Invoke(nameof(MakeAbleToQuit), 1);
        //_blackImage.gameObject.SetActive(false);
    }

    void MakeAbleToQuit()
    {
        _isAbleToQuit = true;
    }
}
