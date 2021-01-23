
public class Constants
{
    public static readonly string HOME_SCENE = "HomeScene";
    public static readonly string THEME_SCENE = "_Theme";
    public static readonly string MENU_SCENE = "_Menu";
    public static readonly string JIGSAW_PUZZLE = "JigsawPuzzle";

    public static readonly int CLASSIC_NUMBER = 17;
    public static readonly int FASHION_NUMBER = 21;
    public static readonly int FREESTYLE_NUMBER = 9;
    public static readonly int GHOST_NUMBER = 10;
    public static readonly int HALLOWEEN_NUMBER = 6;
    public static readonly int KUTE = 12;
    public static readonly int LOL_NUMBER = 19;
    public static readonly int NOEL_NUMBER = 8;

    public static readonly ItemTheme[] itemThemes = new ItemTheme[]
{
    new ItemTheme() {id = 1,nameTheme = "Classic", type = TypeTheme.Classic, url = ""},
    new ItemTheme() {id = 2,nameTheme = "Noel", type = TypeTheme.Noel, url = ""},
    new ItemTheme() {id = 3,nameTheme = "Fashion", type = TypeTheme.Fashion, url = ""},
    new ItemTheme() {id = 4,nameTheme = "Kute", type = TypeTheme.Kute, url = ""},
    new ItemTheme() {id = 5,nameTheme = "Halloween", type = TypeTheme.Halloween, url = ""},
    new ItemTheme() {id = 6,nameTheme = "Lol", type = TypeTheme.Lol, url = ""},
    new ItemTheme() {id = 7,nameTheme = "Classic", type = TypeTheme.Classic, url = ""},
    new ItemTheme() {id = 8,nameTheme = "Ghost", type = TypeTheme.Ghost, url = ""},
    new ItemTheme() {id = 9,nameTheme = "FreeStyle", type = TypeTheme.FreeStyle, url = ""},
};

}

public enum TypeTheme
{
    Classic,
    Fashion,
    FreeStyle,
    Ghost,
    Halloween,
    Kute,
    Lol,
    Noel
}


