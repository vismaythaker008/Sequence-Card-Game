using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

public class BoxScript : MonoBehaviour
{
    public ParticleSystem particle;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().AddForce(Vector3.down * 3000f, ForceMode.Acceleration);
        animator = GetComponent<Animator>();
        animator.SetTrigger(ConstantString.BoxDrop);
    }


    public void PlayParticleEffect()
    {
        if (particle != null)
        {
            if (!particle.isPlaying)
                particle.Play();
            particle.gameObject.GetComponent<CFX_AutoDestructShuriken>().enabled = true;
            //GetComponent<Rigidbody>().isKinematic = true;

        }
    }
    public void PlayBoxOpenanimation()
    {
        animator.SetTrigger(ConstantString.OpenBoxAnimation);

    }
    public void PlayBoxTurnUpsideDownAnimation()
    {
        animator.SetTrigger(ConstantString.BoxTopClose);
        animator.SetTrigger(ConstantString.BoxTurnUpsideDown);
    }
    public void PlayBoxTurnDownsideUpAnimation()
    {
        Debug.Log("downside up");
        animator.SetTrigger(ConstantString.BoxTurnDownsideUp);
    }
    public void PlayBoxBottomAndBoardOpenAnimation()
    {
        animator.SetTrigger(ConstantString.BoxTopBottomMove);
    }
    public void PlayBoardOpenAnimation()
    {
        animator.SetTrigger(ConstantString.OpenBoardAnimation);
    }
    public void ChangeStateToGamePlay()
    {
        GetComponentInChildren<PlayerSelectionMenuUI>().OnPlayButtonClick();
    }
    public void PlayBoxCloseanimation(string mode)
    {
        switch (mode)
        {
            case "Play VS AI":
                animator.SetTrigger(ConstantString.BoxTopClose);
                WaitAsync(1200, () =>
                    {
                        GetComponentInChildren<SelectionMenuUI>().OnModeBtnClk(mode);
                        PlayBoxOpenanimation();
                    });
                break;
            case "Go Back":
                animator.SetTrigger(ConstantString.BoxTopClose);
                WaitAsync(1200, () =>
                    {
                        GetComponentInChildren<PlayerSelectionMenuUI>().OnBackBtnClk();
                        PlayBoxOpenanimation();
                    });
                break;

        }

    }

    IEnumerator Wait(float seconds, Action actiontoperform)
    {
        yield return new WaitForSeconds(seconds);

        if (actiontoperform != null)
        {
            actiontoperform();
        }

    }

    async void WaitAsync(int delayinms, Action actiontoperform)
    {
        await Task.Delay(delayinms);

        if (actiontoperform != null)
        {
            actiontoperform();
        }
    }
}
