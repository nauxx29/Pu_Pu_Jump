using System;

public class EventCenter
{
    public static Event OnRestart = new Event();
    public static Event OnMusicChange = new Event();
    public static Event OnSceneChange = new Event();
}

public class Event
{
    private event Action Evt;

    public void AddListener(Action eventName)
    {
        Evt += eventName;
    }

    public void RemoveListener(Action eventName) 
    {  
        Evt -= eventName; 
    }

    public void Invoke()
    {
        Evt?.Invoke();
    }
}

