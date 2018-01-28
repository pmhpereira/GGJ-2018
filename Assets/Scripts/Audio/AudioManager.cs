using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private static AudioManager s_Instance;
    public static AudioManager Instance { get { return s_Instance; }}


    [SerializeField]
    private AudioSource m_BGM;
    [SerializeField]
    private AudioSource m_SFX;

    private void Awake()
    {
        s_Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayLevelStart(){
        StartCoroutine(PlayIntro());
    }

    public void StopBGM()
    {
        m_BGM.Stop();
    }

    IEnumerator PlayIntro(){

        PlayBGM("gameIntro", true);

        while(m_BGM.isPlaying){
            yield return new WaitForEndOfFrame();
        }

        PlayBGM("gameLoop", true, true);
    }

    public void PlayBGM(string name, bool force = false, bool loop = false){

        if (m_BGM != null)
        {
            if (force)
                m_BGM.Stop();
            else if (m_BGM.isPlaying)
                return;

            AudioClip newClip = (AudioClip)Resources.Load("BGM/" + name);
            if (newClip != null)
            {
                m_BGM.clip = newClip;
                m_BGM.loop = loop;
                m_BGM.Play();
            }
        }

    }

    public void PlaySFX(string name, bool force = false){
        if(m_SFX != null){

            if (force)
                m_SFX.Stop();
            else if (m_SFX.isPlaying)
                    return;

            AudioClip newClip = (AudioClip)Resources.Load("SFX/" + name);
            if (newClip != null)
            {
                m_SFX.clip = newClip;
                m_SFX.Play();
            }
        }
    }
}
