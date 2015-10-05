namespace CSSTerminal.SanitySuitTree.BuildTreecontrol
{
    public class BuildControl
    {
        public bool OpenPropertyWindowDialog()
        {
            BuildPropertyWindow dialog = new BuildPropertyWindow();
            dialog.ShowDialog();
            return true;
        }
    }
}
