using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public Transform m_OptionsContainer;
    public Transform m_Cursor;

    public Button[] options;
    public int currentOption;


    // Use this for initialization
    void Start()
    {
        currentOption = 0;
        if (options == null)
            options = m_OptionsContainer.GetComponentsInChildren<Button>();
    }

    float delay = 0.5f;
    float currentDelay = 0f;

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.gameState == GameController.GameState.Start)
        {
            if (currentDelay <= 0f) //read input
            {
                if (Input.GetAxis("P1_Vertical") > 0.1f || Input.GetAxis("Vertical") > 0.1f)
                {
                    print("UP");
                }
                if (Input.GetAxis("P1_Vertical") < -0.1f || Input.GetAxis("Vertical") < -0.1f)
                {
                    print("DOWN");
                }

                if (Input.GetButton("P1_Jump") || Input.GetMouseButtonDown(0))
                {

                }

                currentDelay = delay;
            }
            else
                currentDelay -= Time.unscaledDeltaTime; //just in case we paused the regular scale..
        }

        UpdateCursor();

    }

    private void UpdateCursor()
    {
        if (options != null && options.Length > 0)
            m_Cursor.transform.localPosition = options[currentOption].transform.position + (Vector3.left * -100f);
    }

    private void NextOption()
    {
        if (currentOption == options.Length - 1)
            return;

        currentOption++;
    }

    private void PrevOption()
    {
        if (currentOption == 0)
            return;

        currentOption--;
    }


}
