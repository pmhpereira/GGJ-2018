using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInfo : MonoBehaviour {

    [System.Serializable]
    public struct InteractionSprites{
        public Data.Player.Interaction type;
        public Sprite sprite;
    }

    public InteractionSprites[] interactionSprites;

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    private Data.Player m_Player;
    private Data.Player.Interaction m_ShowingInteraction;

    public void Init(Data.Player player){
        m_Player = player;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Player != null && m_Player.currentInteractionInfo != m_ShowingInteraction){
            m_ShowingInteraction = m_Player.currentInteractionInfo;
            if (m_ShowingInteraction == Data.Player.Interaction.NotInteractable)
            {
                AudioManager.Instance.PlaySFX("wrong", true);
            }
            m_SpriteRenderer.sprite = _Find();
        }
	}

    private Sprite _Find(){
        foreach(var i in interactionSprites){
            if (i.type == m_ShowingInteraction)
                return i.sprite;
        }
        return null;
    }
}
