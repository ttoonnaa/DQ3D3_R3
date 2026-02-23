
namespace SrLib
{
    public class SrTextComponent : SrComponentBase
    {
        public override void Setup(SrContext context)
        {
            var textComponent = GetComponent<TMPro.TextMeshProUGUI>();
            if (textComponent)
            {
                textComponent.font = context.SystemTmpFont;
            }
        }
    }
}