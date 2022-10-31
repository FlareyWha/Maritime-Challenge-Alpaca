using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildMemberUI : MonoBehaviour
{
    [SerializeField]
    private Text guildMemberName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGuildMemberUI(string name)
    {
        guildMemberName.text = name;
    }
}
