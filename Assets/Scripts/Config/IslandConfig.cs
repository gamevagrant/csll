

public class IslandConfig {

    public string[] buildingNames;
    public string[] islandNames;

    public string GetBuildingName(int index)
    {
        if (index > 0 && index < buildingNames.Length)
        {
            return buildingNames[index-1];
        }
        return "";
    }

    public string GetIslandName(int index)
    {
        if (index > 0 && index < islandNames.Length)
        {
            return islandNames[index-1];
        }
        return "";
    }
}
