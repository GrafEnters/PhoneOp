using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Clock : InteractableObject
{
    public static Clock instance;
    public float SecondsInOneMinute = 1;
    public Transform HoursTransform, MinutesTransform;
    public int hours, minutes;
    public bool isGoing;
    public UnityEvent OnClockClicked;
    AudioSource audioSource;
    public AudioClip click1, click2, ring;
    bool isDayEnded;
    bool firstRingRang;
    float curTime;
    int curTick;

    public UnityAction onStartDay, onEndDay;
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartClock(Settings.config.arriveHour);
    }

    public void StartClock(int startHours, int startMinutes)
    {
        isGoing = true;
        SetTime(startHours, startMinutes);
    }

    public void StartClock(float time)
    {
        isGoing = true;
        SetTime(Mathf.FloorToInt(time), Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 60));
    }
    void StopClock()
    {
        isGoing = false;
        audioSource.PlayOneShot(ring);
    }

    public float GetTime()
    {
        float time = hours + (minutes * 1f / 60);
        return time;
    }

    [HideInInspector]
    public override void OnmouseDown()
    {
        OnClockClicked.Invoke();
    }

    public bool IsWorkTime()
    {
        return GetTime() > Settings.config.startDayHour && GetTime() < Settings.config.endDayHour;
    }

    void SetTime(int hours, int minutes)
    {
        this.hours = hours;
        this.minutes = minutes;
        UpdateArrows();
    }

    void UpdateArrows()
    {
        curTick++;
        audioSource.PlayOneShot((curTick % 2 == 0) ? click1 : click2);
        HoursTransform.localRotation = Quaternion.Euler(0, 0, 30 * hours);
        MinutesTransform.localRotation = Quaternion.Euler(0, 0, 6 * minutes);
    }
    void StartDay()
    {
        isDayEnded = false;
        onStartDay.Invoke();
        audioSource.PlayOneShot(ring);
    }
    void RingBell()
    {
        audioSource.PlayOneShot(ring);
    }

    public void EndDay()
    {
        isDayEnded = true;
        onEndDay?.Invoke();
        SaveManager.sv.currentDay++;
        SaveManager.sv.dayResult.isWorkedAllDay = true;
        SaveManager.Save();
        RingBell();
        Debug.Log("?????????????? ???????? ????????????????. ?????????????????? ?????????????????? ????????????");
    }

    public void Leave()
    {
        
        SceneManager.LoadScene("Menu");
    }

    protected override void Update()
    {
        base.Update();
        if (!isGoing)
        {
            curTime = 0;
            return;
        }

        curTime += Time.deltaTime;
        if (curTime >= SecondsInOneMinute)
        {
            curTime = 0;
            minutes++;
            if (minutes > 59)
            {
                minutes = 0;
                hours++;
            }
            UpdateArrows();
        }
        if (hours + minutes / 60f > Settings.config.startDayHour && !firstRingRang)
        {
            StartDay();
            firstRingRang = true;
        }

        if (hours + minutes / 60f > Settings.config.leaveHour && !isDayEnded)
        {
            EndDay();
            if (Settings.config.isInstaExitOnEndOfDay)
                Leave();
        }
    }
}