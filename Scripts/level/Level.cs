public class Level{
    public string Name{ get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public string[] Blocks {get; set; }

    public LevelLayout Layout { get; set; }

    public struct LevelLayout{
        public int[][] Blocks {get; set;}
    }

}