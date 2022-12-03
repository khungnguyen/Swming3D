using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StudentExtension : MonoBehaviour
{
    // Start is called before the first frame update
    public KickBoardController kickBoardController;

    public List<Transform> extensionTransforms;

    enum Extension
    {
        KickBoardExtension
    }
    void Start()
    {

    }
    public void UpdateExtensions(string[] extension, string trigger, string lastTrigger)
    {
        if (extension != null)
        {
            for (var i = 0; i < extension.Length; i++)
            {
                if (extension[i] == Extension.KickBoardExtension.ToString())
                {
                    if (kickBoardController != null)
                    {
                        kickBoardController.SetBehavior(trigger, lastTrigger);
                    }

                }
            }
        }


    }
    public void SetUpBaseOnLesson(int lesson)
    {
        if (kickBoardController != null)
        {
            kickBoardController.HideKickBoards();
        }

    }
    public void ActivateExtension(string trigger, bool active)
    {
        var transform = extensionTransforms.Find(e => e.name == trigger);
        if (transform != null)
        {
            transform.gameObject.SetActive(active);
        }
    }
    public void HideAllExtension()
    {
        if (kickBoardController != null)
        {
            kickBoardController.HideKickBoards();
        }
        extensionTransforms.ForEach(e =>
        {
            e.gameObject.SetActive(false);
        });
    }
}
