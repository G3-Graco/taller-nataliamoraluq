using Newtonsoft.Json.Linq;

namespace TallerBlazorApp.Components
{
    public class TokenContainer
    {
        //token cont --- base del repo del prof. Guille
        public string? token { get; private set; } = string.Empty;

        public void AsignarToken(string newToken)
        {
            token = newToken;
            NotifyStateChanged();
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void Clear()
        {
            token = null;
            NotifyStateChanged();

        }
    }
}
