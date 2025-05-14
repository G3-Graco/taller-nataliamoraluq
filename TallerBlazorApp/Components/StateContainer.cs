namespace TallerBlazorApp.Components
{
    public class StateContainer
    {
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
