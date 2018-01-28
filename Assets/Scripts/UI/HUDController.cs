using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField]
    private Slider player1Life;
    [SerializeField]
    private Slider player2Life;

    [SerializeField]
    private AnimationCurve m_IdolTransferScaleCurve;
    [SerializeField]
    private Image m_IdolTransferPrefab;

    private Data.Player[] m_Players;

    public void Init(float maxLife, Data.Player[] players){
        m_Players = players;
        player1Life.maxValue = player2Life.maxValue = maxLife;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(m_Players != null){

            foreach(var p in m_Players){

                if (p.playerIndex == 0)
                {
                    player1Life.value = p.life;
                    player1Life.handleRect.gameObject.SetActive(p.hasItem);
                }
                else
                {
                    player2Life.value = p.life;
                    player2Life.handleRect.gameObject.SetActive(p.hasItem);
                }
                
            }
        }

	}

    private LTDescr m_IdolTween;

    public void AnimateIdolTransfer(Vector3 origin, Vector3 destination, System.Action cb = null){

        if (m_IdolTween != null)
            return;

        Image idolInstance = Instantiate(m_IdolTransferPrefab, transform.parent);
        idolInstance.transform.SetAsLastSibling();

        m_IdolTween = LeanTween.value(idolInstance.gameObject, (float val) =>
        {
            idolInstance.transform.position = Vector3.Lerp(origin, destination, val);
            idolInstance.transform.localScale = Vector3.one * m_IdolTransferScaleCurve.Evaluate(val);
        }, 0f, 1f, 0.75f).setOnComplete(()=>
            {
                m_IdolTween = null; // safe???
                Destroy(idolInstance.gameObject);
                if (cb != null)
                    cb();
            }).setIgnoreTimeScale(true);
    }
}
