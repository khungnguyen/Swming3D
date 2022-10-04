using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
   private TMP_Text label;
   private Player player;
public void setPlayerInfo(Player p){
    player = p;
    label.text = p.NickName;
}
}
