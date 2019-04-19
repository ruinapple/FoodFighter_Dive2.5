using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;


public class TimerCtrl : MonoBehaviour
{
    private const string start = "Start!!";
    private const string ready = "Ready..";
    private bool isStarted;
   // private int leftTime;


    public Text timeTxt;

    public AudioSource _audio;
    public AudioClip[] startSoundEffect;
    public VideoPlayer player;
    public CanvasGroup scoreBoard;
    public Text bestScore;
    public Text yourScore;
    public GameObject btnGroup;
    public GameObject skipBtn;
    public float startTime = 35.5f;
    public float playTime = 120.0f;




    void Start()
    {
        timeTxt.GetComponent<Text>();
        _audio = GetComponent<AudioSource>();  
     //   StartCoroutine ("PlayList");
    }



    void Update()
    {
        //PlayerPrefs.DeleteAll();
        if (GameManager.Instance.isTimeOver) return; //??? 클락타임이 양의방향이 아니라 음의 방향으로??


        if (player.clockTime < startTime) // 플레이어.비디오플레이어 클래스의 클락타임이 만약 스타트 타임보다 작아지면
        {
            timeTxt.text = ready + " " + (int)(startTime - player.clockTime + 1); //35.5초에서 줄어듬. 타임텍스트에 레디+ 스타트타임에서 비디오클래스 클락타임이 늘어난만큼 빼서 표시해라

            if ((int)(startTime - player.clockTime) ==3f)
            {
                _audio.clip = startSoundEffect[0];  //  사운드
                _audio.volume = 0.3f;
                _audio.Play();
             //_audio.PlayOneShot(startSoundEffect[0], 0.5f); 
            }

            GameManager.Instance.itemState = ItemState.WAIT; // 레티클 상태 (플레이어상태) 인터렉티브 효과 안나타남
           
           
            if (startTime - player.clockTime + 1 <= 4 && skipBtn.activeSelf)
            {
              skipBtn.SetActive(false);
            }
        }
        else
        {
            
            timeTxt.text = start;  // 스타트 글씨 생겨나고 시작
            if (!isStarted) // ?????? 왜 bool 타입으로? 게임매니저로 인터렉티브 효과를 넣을지 ????
            {
               
                GameManager.Instance.itemState = ItemState.NORMAL; // ?????레티클 상태 (플레이어상태)???? 웨이트와 노말을 차이?
                isStarted = true;
                StartCoroutine(DelayTime());

            }
            int leftTime = (int)((playTime - player.clockTime + startTime + 1) * 1000);   // ??? 총 플레이시간에서 남은시간 밀리세컨드로해서 곱천

            //if (timeTxt.text == (@"02\:55\:ff"))
            //{
            //    _audio.clip = startSoundEffect[1];  //  사운드
            //    _audio.volume = 0.7f;
            //    _audio.Play();
            //}

            if (leftTime <= 0) //레프트 타임이 0이 되면 
            //if(leftTime <= 110000)
            {
                timeTxt.text = "Times Up!"; // 타임업 글씨가 생겨난다.
                GameManager.Instance.isTimeOver = true;  // ?????
                StartCoroutine(ScoreBoard()); // 스코어 보드를 돌려라. 
            }
            else if (leftTime < playTime * 1000)
            {
                var timeSpan = TimeSpan.FromMilliseconds(leftTime);
                timeTxt.text = timeSpan.ToString(@"mm\:ss\:ff"); // 포맷으로 나눠주기 분, 초, .. 타임스판..
            }
        }
        
    }
    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(4.0f);
        _audio.clip = startSoundEffect[1];  //  사운드
        _audio.volume = 0.05f;
        _audio.Play();
    }

    private IEnumerator ScoreBoard()
    {
        float currVolume = player.GetDirectAudioVolume(0);
        while (!Mathf.Approximately(scoreBoard.alpha, 1.0f))
        {
            player.SetDirectAudioVolume(0,  (1 - scoreBoard.alpha) * currVolume);
            scoreBoard.alpha = Mathf.MoveTowards(scoreBoard.alpha, 1.0f, Time.deltaTime);
            yield return null;
        }
        _audio.clip = startSoundEffect[2];  // 끝나는 사운드
        _audio.Play(); //게임 끝나고 생기는 사운드
        int bScore = PlayerPrefs.GetInt("BestScore", 0);
        string best = "Your Best Score " + bScore + " Kcal";
        bestScore.text = best;
        yield return new WaitForSeconds(1.0f);
        string yours = "";
        if(bScore < GameManager.Instance.Kcal)
        {
            yours = "The New Record!! " + GameManager.Instance.Kcal + " Kcal";
            PlayerPrefs.SetInt("BestScore", GameManager.Instance.Kcal);
            PlayerPrefs.Save();
        }
        else
        {
            yours = "Oops Try Again!!";
        }
        yourScore.text = yours;
        yield return new WaitForSeconds(1.0f);
        btnGroup.SetActive(true);
    }

    //IEnumerator PlayList()
    //{
    //    _audio.clip = startSoundEffect[0]; //배열 0번째를 실행한다.
    //    _audio.Play(); //객체 audiosource를 통해 사운드를 재생한다.
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(1.0f); //1초마다 사운드가 재생여부를 확인해라
    //        if(!_audio.isPlaying) //사운드가 종료되면 실행해라
    //        {
    //            _audio.clip = startSoundEffect[1]; //0번사운드종료시 1번사운드를 실행해라
    //            _audio.Play(); //객체 audiosource를 통해 사운드를 재생한다.
    //            _audio.loop = false; //오디오소스루프를 통해 반복재생할지 설정한다. 
    //        }
    //    }
    //}
}
