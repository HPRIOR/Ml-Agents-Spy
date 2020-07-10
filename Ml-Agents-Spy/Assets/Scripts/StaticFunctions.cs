public static class StaticFunctions
{
    public static int MapScaleToMatrixSize(int mapScale) =>
        mapScale % 2 == 0 ? (mapScale * 10) / 2 : ((mapScale * 10) / 2) + 1;
}
