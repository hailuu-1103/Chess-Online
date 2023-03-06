namespace Utils
{
    using System;
    using TMPro;

    public static class FormatNumberHelper
    {
        public static void SetRemainTime(this TextMeshProUGUI txtCoolDown, float remainTime) { txtCoolDown.text = TimeSpan.FromSeconds(remainTime).ToString(@"mm\:ss"); }
    }
}