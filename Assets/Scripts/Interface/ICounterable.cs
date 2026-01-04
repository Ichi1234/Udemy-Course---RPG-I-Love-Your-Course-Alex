using UnityEngine;

public interface ICounterable 
{
    public bool CanbeCounter { get; }
    public void HandleCounter();
}
