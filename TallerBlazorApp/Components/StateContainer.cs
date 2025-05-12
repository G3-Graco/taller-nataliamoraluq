namespace TallerBlazorApp.Components
{
    public class StateContainer
    {
        //state container; --- base del repo del prof. Guille
        public string SelectedCssClass { get; private set; }

        public event Action CambiarColor;

        public void AsignarColorCss(string newCssClass)
        {
            SelectedCssClass = newCssClass;
            ExecuteAction();
        }

        private void ExecuteAction() => CambiarColor?.Invoke();
    }
}
