using Microsoft.AspNetCore.Components;

namespace DreamNumbers.UI.Components
{
    public partial class TabControl : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }

        internal List<Tab> Tabs { get; } = new();
        internal Tab? ActiveTab { get; set; }

        internal void Register(Tab tab)
        {
            if (!Tabs.Contains(tab))
            {
                Tabs.Add(tab);

                if (ActiveTab is null)
                    ActiveTab = tab;

                StateHasChanged();
            }
        }

        private void SelectTab(Tab tab)
        {
            ActiveTab = tab;
            StateHasChanged();
        }
    }
}
