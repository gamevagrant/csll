

public class IslandConfig {

    public string[] buildingNames;
    public string[] islandNames;

    /// <summary>
    /// index 从0开始
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetBuildingName(int index)
    {
        if (index >= 0 && index < buildingNames.Length)
        {
            return buildingNames[index];
        }
        return "";
    }
    /// <summary>
    /// index 从1开始
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetIslandName(int index)
    {
        if (index > 0 && index <= islandNames.Length)
        {
            return islandNames[index-1];
        }
        return "";
    }
}
