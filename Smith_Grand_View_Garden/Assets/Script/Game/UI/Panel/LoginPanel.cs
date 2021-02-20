using DIY.UI;
using UnityEngine;

namespace Game.UI.Panel
{
    public class LoginPanel : Base_UIPanel
    {
        public override void InitBasicInfo()
        {
            this.name = this.GetType().ToString();
            this.path = "aaaaaa";
        }

        public override UITree InitUITree()
        {
            UITree uiTree_launch = UIUtil.AutoCreateUITree(
            new UINode("Button_begin"),
            new UINode("Button_change"),
            new UINode("Button_set"),
            new UINode("Button_exit"),
            new UINode("Panel_logo",
                new UINode("Image_gender"),
                new UINode("Image_name"),
                new UINode("Image_icon")));
            uiTree_launch["Button_begin"].BindEvent_Button(() =>
            {
                uiTree_launch["Image_icon"].ChangeSpriteColor(Color.red);
            });
            uiTree_launch["Button_exit"].BindEvent_Button(() =>
            {
                uiTree_launch["Image_icon"].ChangeSpriteColor(Color.yellow);
            });
            return uiTree_launch;
        }

        public override void Show()
        {
            base.Show();
            uiTree["Image_name"].ChangeSpriteColor(Color.blue);
        }

        public override void Freeze()
        {
            base.Freeze();
        }

    }
}
