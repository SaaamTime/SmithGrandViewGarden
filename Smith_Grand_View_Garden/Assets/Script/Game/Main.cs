using DIY.Time;
using DIY.UI;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform launchPanel;
    void Start()
    {
        Debug.Log(TimeUtil.GetTimestamp());
        //UITree uiTree_launch = UIUtil.AutoCreateUITree(launchPanel,
        //    new UINode("Button_begin"),
        //    new UINode("Button_change"),
        //    new UINode("Button_set"),
        //    new UINode("Button_exit"),
        //    new UINode("Panel_logo",
        //        new UINode("Image_gender"),
        //        new UINode("Image_name"),
        //        new UINode("Image_icon")));
        //uiTree_launch["Button_begin"].BindEvent_Button(() =>
        //{
        //    uiTree_launch["Image_icon"].ChangeSpriteColor(Color.red);
        //});
        //uiTree_launch["Button_exit"].BindEvent_Button(() =>
        //{
        //    uiTree_launch["Image_icon"].ChangeSpriteColor(Color.yellow);
        //});


        Transform Button_begin = launchPanel.Find("Button_begin");
        Transform Button_change = launchPanel.Find("Button_change");
        Transform Button_set = launchPanel.Find("Button_set");
        Transform Button_exit = launchPanel.Find("Button_exit");
        Transform Panel_logo = launchPanel.Find("Panel_logo");
        Transform Image_gender = Panel_logo.Find("Image_gender");
        Transform Image_name = Panel_logo.Find("Image_name");
        Transform Image_icon = Panel_logo.Find("Image_icon");
        Button Button_begin_a = (Button)Button_begin.GetComponent(typeof(Button));
        Button Button_exit_a = (Button)Button_exit.GetComponent(typeof(Button));
        Image icon = (Image)Image_icon.GetComponent(typeof(Image));
        Button_begin_a.onClick.RemoveAllListeners();
        Button_begin_a.onClick.AddListener(() => { icon.color = Color.red; });
        Button_exit_a.onClick.RemoveAllListeners();
        Button_exit_a.onClick.AddListener(() => { icon.color = Color.yellow; });

        Debug.Log(TimeUtil.GetTimestamp());
        Debug.Log("游戏启动");
    }
}
