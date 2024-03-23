using UnityEngine;
using UnityEngine.Video;

public class CutSceneManager : MonoBehaviour
{
    private string evilMentorPath = "/Cutscenes/EvilMentor.mp4";
    private VideoPlayer fullScreenVideoPlayer;

    public static CutSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        evilMentorPath = Application.dataPath + evilMentorPath;
    }

    //1
    public void PlayFullScreenOfflineVideo()
    {
        StartFullScreenVideo(evilMentorPath);
    }

    private void StartFullScreenVideo(string path)
    {
        //2
        if (fullScreenVideoPlayer)
        {
            Destroy(fullScreenVideoPlayer);
        }

        //3
        fullScreenVideoPlayer = Camera.main.gameObject.AddComponent<VideoPlayer>();

        //4
        ResetManager.Instance.DisablePower(true);
        ResetManager.Instance.DisableFairyMovement(true);
        ResetManager.Instance.DisableWizardInput(true);
        GameUIHandler.Instance.TurnOffGameUI();

        //5
        fullScreenVideoPlayer.playOnAwake = false;
        fullScreenVideoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        fullScreenVideoPlayer.url = path;
        fullScreenVideoPlayer.frame = 0;
        fullScreenVideoPlayer.isLooping = false;
        fullScreenVideoPlayer.loopPointReached += StopVideo;

        //6
        fullScreenVideoPlayer.Play();
    }

    private void StopVideo(VideoPlayer vp)
    {
        vp.Stop();
        TutorialManager.levelOver = true;
    }
}