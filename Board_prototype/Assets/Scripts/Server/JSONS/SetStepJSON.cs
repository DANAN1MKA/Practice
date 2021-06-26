using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetStepJSON
{
    public string gameID;
    public string playerID;
    public int damageAmount;
    public LiteElemet[] board;
    public SwipeHistory[] swipeHistory;
    public int[] newGemsType;

    public SetStepJSON(string _gameID,string playerID, int _damaeAmount, LiteElemet[] _board, SwipeHistory[] _swipeHistry, int[] _newGemsType)
    {
        this.gameID = _gameID;
        this.playerID = playerID;
        this.damageAmount = _damaeAmount;
        this.board = _board;
        this.swipeHistory = _swipeHistry;
        this.newGemsType = _newGemsType;
    }
    public SetStepJSON(int _damageAmount, LiteElemet[] _board, SwipeHistory[] _swipeHistry, int[] _newGemsType)
    {
        this.damageAmount = _damageAmount;
        this.board = _board;
        this.swipeHistory = _swipeHistry;
        this.newGemsType = _newGemsType;
    }

}
