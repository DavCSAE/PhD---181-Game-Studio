using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetManager : MonoBehaviour
{

    public static PlayerTargetManager Singleton;

    public List<PlayerTarget> targets = new List<PlayerTarget>();

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTarget(PlayerTarget target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(PlayerTarget target)
    {
        targets.Remove(target);
        Player.Singleton.targeting.TargetHasBeenRemoved(target);
    }

}
