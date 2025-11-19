using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<string, Action> eventDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            eventDictionary = new Dictionary<string, Action>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        if (Instance.eventDictionary.ContainsKey(eventName))
        {
            Instance.eventDictionary[eventName] += listener;
        }
        else
        {
            Instance.eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (Instance.eventDictionary.ContainsKey(eventName))
        {
            Instance.eventDictionary[eventName] -= listener;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Event {eventName} not found!");
        }
    }
}