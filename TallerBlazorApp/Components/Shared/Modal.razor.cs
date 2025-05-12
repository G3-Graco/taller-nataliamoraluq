using Microsoft.AspNetCore.Components;

namespace TallerBlazorApp.Components.Shared
{
    public partial class Modal
    {
        
        public string mostrarModal { get; set; } = "none";
        public bool showBackdrop { get; set; }

        [Parameter]
        public string titulo { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        public void Open()
        {
            mostrarModal = "block";
            showBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            mostrarModal = "none";
            showBackdrop = false;
            StateHasChanged();

        }
    }
}
