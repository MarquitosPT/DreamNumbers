using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Components
{
    public partial class Tab : ComponentBase
    {
        [CascadingParameter] public TabControl? Parent { get; set; }

        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void OnInitialized()
        {
            Parent?.Register(this);
        }
    }
}
