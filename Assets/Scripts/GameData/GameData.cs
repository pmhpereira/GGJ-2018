using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Data {

    [Serializable]
    public class Player {

        public float itemSpeedModifier = 0.75f; // 3/4 speed when carrying shit

        public bool hasItem { get { return m_HasItem; }}
        public int playerIndex { get { return m_PlayerIndex; } }
        public float life { get { return m_Life; }}
        public float speed { get { return hasItem ? m_Speed * itemSpeedModifier : m_Speed; }}

        private bool m_HasItem;
        private int m_PlayerIndex;
        private float m_Life; //0f - 1f
        private float m_Speed;

        public Player(int index, float life, float speed){
            m_Life = life;
            m_PlayerIndex = index;
            m_Speed = speed;
        }


    }

    public class Game
    {


        public Game(){
            
        }
    }



}
