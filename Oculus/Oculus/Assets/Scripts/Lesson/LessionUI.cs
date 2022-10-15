using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessionUI : MonoBehaviour,IButtonAction
{
    public GameObject lessonButtonPrefab;

    public GameObject PanleUserDecison;

    public Transform scrollContent;

    public void OnClicked(string action)
    {
        PanleUserDecison.SetActive(true);
        ExerciseManager.instance.SetExercises(LessonManager.instance.lessonData[int.Parse(action)].text);
        transform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
       for(var i = 0; i < LessonManager.instance.lessonData.Length; i++ )
        {
           var go = Instantiate(lessonButtonPrefab, scrollContent);
            var comp = go.GetComponent<LessonButton>();
            comp.SetText(LessonManager.instance.lessonData[i].name);
            comp.SetButtonInfo(i);
            comp.OnClicked += OnClicked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
