using MudBlazor;

namespace ProjectManager.Client.Shared
{
    public class CustomTheme : MudTheme
    {
        public CustomTheme()
        {
            //TODO
            Palette = new PaletteLight()
            {
                Primary = Colors.Shades.Black,
                Secondary = Colors.Shades.White,
                Info = Colors.Indigo.Darken4,
                AppbarBackground = Colors.Indigo.Darken4,
            };
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Blue.Lighten1
            };
        }
    }
}
