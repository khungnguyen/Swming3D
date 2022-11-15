using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentExtension : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private KickBoardController kickBoardController;

    [SerializeField]
    private List<Transform> extensionTransforms;
    void Start()
    {

    }
    public void UpdateExtensions(int lesson, string trigger, string lastTrigger)
    {
        if (lesson == (int)LessonID.Lesson_2)
        {
            kickBoardController.SetBehavior(trigger, lastTrigger);
        }

    }
    public void SetUpBaseOnLesson(int lesson)
    {
        kickBoardController.HideKickBoards();
    }
    public void ActivateExtension(string trigger, bool active)
    {
        var transform = extensionTransforms.Find(e=>e.name == trigger);
        if(transform != null) {
            transform.gameObject.SetActive(active);
        }
    }
    public void HideAllExtension()
    {
        kickBoardController.HideKickBoards();
        extensionTransforms.ForEach(e =>
        {
            e.gameObject.SetActive(false);
        });
    }
}
