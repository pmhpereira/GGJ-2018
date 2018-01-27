using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Data
{

    [Serializable]
    public class Player
    {
        public float itemSpeedModifier = 0.75f; // 3/4 speed when carrying shit
        public float runSpeedModifier = 1.25f;

        public bool wantsSwitch { get { return m_WantsSwitch; } set { m_WantsSwitch = value; }}
        public bool hasItem { get { return m_HasItem; } set { m_HasItem = value; } }
        public bool isRunning { get { return m_IsRunning; } set { m_IsRunning = value; } }
        public int playerIndex { get { return m_PlayerIndex; } }
        public float life { get { return m_Life; } set { m_Life = value; } }
        public float speed
        {
            get
            {
                if (hasItem)
                    return m_BaseSpeed * itemSpeedModifier;
                else if (isRunning)
                    return m_BaseSpeed * runSpeedModifier;
                else
                    return m_BaseSpeed;
            }
        }

        private bool m_WantsSwitch;
        private bool m_HasItem;
        private bool m_IsRunning;
        private int m_PlayerIndex;
        private float m_Life; //in seconds
        private float m_BaseSpeed;

        public Player(int index, float life, float baseSpeed)
        {
            m_Life = life;
            m_PlayerIndex = index;
            m_BaseSpeed = baseSpeed;
        }
    }

    [Serializable]
    public class LevelPair{
        public Transform p1Level;
        public Transform p2Level;

        public void DestroyLevels(){
            UnityEngine.Object.Destroy(p1Level.gameObject);
            UnityEngine.Object.Destroy(p2Level.gameObject);
        }
    }

}
