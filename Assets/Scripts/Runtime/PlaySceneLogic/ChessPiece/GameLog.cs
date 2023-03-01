﻿using Runtime.PlaySceneLogic.ChessPiece;
using Runtime.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Runtime.PlaySceneLogic.ChessPiece
{
    public class GameLog
    {
        public string Id;
        public DateTime Time;
        public GameResultStatus Status;
        public PieceTeam winTeam;

        public GameLog()
        {
        }

        public GameLog(string id, DateTime time, GameResultStatus status, PieceTeam winTeam)
        {
            Id = id;
            Time = time;
            Status = status;
            this.winTeam = winTeam;
        }
    }
}
