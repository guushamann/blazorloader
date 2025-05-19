using System;
using System.Collections.Generic;

public class DataChangeNotifier
{
    private readonly Dictionary<string, List<Action>> _subscriptions = new();

    public void Subscribe(string key, Action callback)
    {
        if (!_subscriptions.ContainsKey(key))
        {
            _subscriptions[key] = new List<Action>();
        }
        _subscriptions[key].Add(callback);
    }

    public void Notify(string key)
    {
        if (_subscriptions.ContainsKey(key))
        {
            foreach (var callback in _subscriptions[key])
            {
                callback.Invoke();
            }
        }
    }
}