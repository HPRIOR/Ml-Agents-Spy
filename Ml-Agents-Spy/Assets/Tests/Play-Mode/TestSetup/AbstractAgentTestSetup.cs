using System.Collections.Generic;
using System.Linq;
using Enums;
using Training;

namespace Tests.TestSetup
{
    public class AbstractAgentTestSetup : AbstractTestTrainingScenarioSetup
    {
        protected List<(float, float)> GetPositionsAroundCenter(TrainingInstanceController trainingInstanceController, int mid, int mapScale)
        {
            var max = StaticFunctions.GetMaxLocalDistance(mapScale);
            
            var position1X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid-1)).Position, trainingInstanceController).x);

            var position1Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid-1)).Position, trainingInstanceController).z);

            var position2X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid)).Position, trainingInstanceController).x);

            var position2Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid)).Position, trainingInstanceController).z);
            
            var position3X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid+1)).Position, trainingInstanceController).x);

            var position3Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid-1, mid+1)).Position, trainingInstanceController).z);
            
             var position4X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid, mid+1)).Position, trainingInstanceController).x);

            var position4Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid, mid+1)).Position, trainingInstanceController).z);
             
            var position5X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid, mid-1)).Position, trainingInstanceController).x);

            var position5Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid, mid-1)).Position, trainingInstanceController).z);
            
            var position6X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid+1)).Position, trainingInstanceController).x);

            var position6Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid+1)).Position, trainingInstanceController).z);
            
            var position7X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid)).Position, trainingInstanceController).x);

            var position7Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid)).Position, trainingInstanceController).z);
            
            var position8X = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid-1)).Position, trainingInstanceController).x);

            var position8Y = StaticFunctions.NormalisedFloat(-max, max, VectorConversions.GetLocalPosition(trainingInstanceController
                .TileDict[TileType.FreeTiles]
                .First(tile => tile.Coords == (mid+1, mid-1)).Position, trainingInstanceController).z);
            
            return new List<(float, float)>()
            {
                (position1X, position1Y),
                (position2X, position2Y),
                (position3X, position3Y),
                (position4X, position4Y),
                (position5X, position5Y),
                (position6X, position6Y),
                (position7X, position7Y),
                (position8X, position8Y)
            };
        }

    }
}