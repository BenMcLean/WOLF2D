using System;
using WarpWriter.Model.Fetch;
using WarpWriter.Model.Seq;
using WarpWriter.View.Render;

namespace WarpWriter.View
{
    public static class PixelCubeDraw
    {
        public static T PixelCube<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCube(model, renderer);
            return renderer;
        }

        public static void PixelCube<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeRight(model, renderer);
        }

        public static T PixelCubeRight<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeRight(model, renderer);
            return renderer;
        }

        public static void PixelCubeRight<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeRight(model, renderer, 6, 6);
        }

        public static T PixelCubeRight<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeRight(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeRight<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            uint sizeX = model.SizeX, sizeY = model.SizeY, sizeZ = model.SizeZ;
            for (int z = 0; z < sizeZ; z++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        byte v = model.At(x, y, z);
                        if (v != 0)
                        {
                            renderer.RectRight(y * scaleX, z * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCubeRightPeek<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeRightPeek(model, renderer);
            return renderer;
        }

        public static void PixelCubeRightPeek<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeRightPeek(model, renderer, 6, 6);
        }

        public static T PixelCubeRightPeek<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeRightPeek(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeRightPeek<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            uint sizeX = model.SizeX, sizeY = model.SizeY, sizeZ = model.SizeZ;
            for (int z = 0; z < sizeZ; z++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        byte v = model.At(x, y, z);
                        if (v != 0)
                        {
                            renderer.RectRight(y * scaleX, z * scaleY, scaleX, scaleY, v);
                            if (z >= sizeZ - 1 || model.At(x, y, z + 1) == 0)
                                renderer.RectVertical(y * scaleX, (z + 1) * scaleY - 1, scaleX, 1, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCubeLeft<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeLeft(model, renderer);
            return renderer;
        }

        public static void PixelCubeLeft<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeLeft(model, renderer, 6, 6);
        }

        public static T PixelCubeLeft<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeLeft(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeLeft<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            uint sizeX = model.SizeX, sizeY = model.SizeY, sizeZ = model.SizeZ;
            for (int z = 0; z < sizeZ; z++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int x = (int)sizeX - 1; x >= 0; x++)
                    {
                        byte v = model.At(x, y, z);
                        if (v != 0)
                        {
                            renderer.RectLeft(y * scaleX, z * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCubeTop<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeTop(model, renderer);
            return renderer;
        }

        public static void PixelCubeTop<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeTop(model, renderer, 6, 6);
        }

        public static T PixelCubeTop<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeTop(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeTop<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            uint sizeX = model.SizeX, sizeY = model.SizeY, sizeZ = model.SizeZ;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = (int)sizeZ - 1; z >= 0; z--)
                    {
                        byte v = model.At(x, y, z);
                        if (v != 0)
                        {
                            renderer.RectVertical(y * scaleX, z * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCubeBottom<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeBottom(model, renderer);
            return renderer;
        }

        public static void PixelCubeBottom<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeBottom(model, renderer, 6, 6);
        }

        public static T PixelCubeBottom<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeBottom(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeBottom<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            uint sizeX = model.SizeX, sizeY = model.SizeY, sizeZ = model.SizeZ;
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    for (int z = 0; z < sizeZ; z--)
                    {
                        byte v = model.At(x, y, z);
                        if (v != 0)
                        {
                            renderer.RectVertical(y * scaleX, z * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCube45<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCube45(model, renderer);
            return renderer;
        }

        public static void PixelCube45<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCube45(model, renderer, 6, 6);
        }

        public static T PixelCube45<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCube45(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCube45<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            byte v;
            int sizeX = (int)model.SizeX,
                    sizeY = (int)model.SizeY,
                    sizeZ = (int)model.SizeZ,
                    pixelWidth = sizeX + sizeY;
            for (int py = 0; py < sizeZ; py++)
            { // pixel y
                for (int px = 0; px <= pixelWidth; px += 2)
                { // pixel x
                    bool leftDone = false, rightDone = pixelWidth - px < 2;
                    int startX = px > sizeX - 1 ? 0 : sizeX - px - 1,
                            startY = px - sizeX + 1 < 0 ? 0 : px - sizeX + 1;
                    for (int vx = startX, vy = startY;
                         vx <= sizeX && vy <= sizeY;
                         vx++, vy++)
                    { // vx is voxel x, vy is voxel y
                        if (!leftDone && vy != 0)
                        {
                            v = model.At(vx, vy - 1, py);
                            if (v != 0)
                            {
                                renderer.RectRight(px * scaleX, py * scaleY, scaleX, scaleY, v);
                                leftDone = true;
                            }
                        }
                        if (!rightDone && vx > 0)
                        {
                            v = model.At(vx - 1, vy, py);
                            if (v != 0)
                            {
                                renderer.RectLeft((px + 1) * scaleX, py * scaleY, scaleX, scaleY, v);
                                rightDone = true;
                            }
                        }
                        if (leftDone && rightDone) break;
                        v = model.At(vx, vy, py);
                        if (v != 0)
                        {
                            if (!leftDone)
                                renderer.RectLeft(px * scaleX, py * scaleY, scaleX, scaleY, v);
                            if (!rightDone)
                                renderer.RectRight((px + 1) * scaleX, py * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCube45Peek<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCube45Peek(model, renderer);
            return renderer;
        }

        public static void PixelCube45Peek<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCube45Peek(model, renderer, 4, 6);
        }

        public static T PixelCube45Peek<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCube45Peek(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCube45Peek<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            byte v;
            int sizeX = (int)model.SizeX,
                    sizeY = (int)model.SizeY,
                    sizeZ = (int)model.SizeZ,
                    pixelWidth = sizeX + sizeY;
            for (int py = 0; py < sizeZ; py++)
            { // pixel y
                for (int px = 0; px <= pixelWidth; px += 2)
                { // pixel x
                    bool leftDone = false, rightDone = pixelWidth - px < 2;
                    int startX = px >= sizeX ? 0 : sizeX - px - 1,
                            startY = px < sizeX ? 0 : px - sizeX + 1;
                    for (int vx = startX, vy = startY;
                         vx <= sizeX && vy <= sizeY;
                         vx++, vy++)
                    { // vx is voxel x, vy is voxel y
                        if (!leftDone && vy > 0 && vx < sizeX)
                        {
                            v = model.At(vx, vy - 1, py);
                            if (v != 0)
                            {
                                renderer.RectRight(px * scaleX, py * scaleY, scaleX, scaleY, v);
                                if (py >= sizeZ - 1 || model.At(vx, vy - 1, py + 1) == 0)
                                    renderer.RectVertical(px * scaleX, (py + 1) * scaleY - 1, scaleX, 1, v);
                                leftDone = true;
                            }
                        }
                        if (!rightDone && vx > 0 && vy < sizeY)
                        {
                            v = model.At(vx - 1, vy, py);
                            if (v != 0)
                            {
                                renderer.RectLeft((px + 1) * scaleX, py * scaleY, scaleX, scaleY, v);
                                if (py >= sizeZ - 1 || model.At(vx - 1, vy, py + 1) == 0)
                                    renderer.RectVertical((px + 1) * scaleX, (py + 1) * scaleY - 1, scaleX, 1, v);
                                rightDone = true;
                            }
                        }
                        if ((leftDone && rightDone) || vx >= sizeX || vy >= sizeY) break;
                        v = model.At(vx, vy, py);
                        if (v != 0)
                        {
                            bool peek = py >= sizeZ - 1 || model.At(vx, vy, py + 1) == 0;
                            if (!leftDone)
                            {
                                renderer.RectLeft(px * scaleX, py * scaleY, scaleX, scaleY, v);
                                if (peek)
                                    renderer.RectVertical(px * scaleX, (py + 1) * scaleY - 1, scaleX, 1, v);
                            }
                            if (!rightDone)
                            {
                                renderer.RectRight((px + 1) * scaleX, py * scaleY, scaleX, scaleY, v);
                                if (peek)
                                    renderer.RectVertical((px + 1) * scaleX, (py + 1) * scaleY - 1, scaleX, 1, v);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static T PixelCubeAbove<T>(this T renderer, IModel model) where T : IRectangleRenderer<T>
        {
            PixelCubeAbove(model, renderer);
            return renderer;
        }

        public static void PixelCubeAbove<T>(IModel model, T renderer) where T : IRectangleRenderer<T>
        {
            PixelCubeAbove(model, renderer, 6, 2);
        }

        public static T PixelCubeAbove<T>(this T renderer, IModel model, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            PixelCubeAbove(model, renderer, scaleX, scaleY);
            return renderer;
        }

        public static void PixelCubeAbove<T>(IModel model, T renderer, int scaleX, int scaleY) where T : IRectangleRenderer<T>
        {
            int sizeX = (int)model.SizeX,
                    sizeY = (int)model.SizeY,
                    sizeZ = (int)model.SizeZ,
                    pixelHeight = (sizeX + sizeZ) * 2;
            for (int vy = 0; vy < sizeY; vy++)
            { // voxel y is pixel x
              // Begin bottom row
                byte v = model.At(0, vy, 0);
                if (v != 0) renderer.RectRight(vy * scaleX, 0, scaleX, scaleY, v);
                // Finish bottom row
                // Begin main bulk of model
                for (int py = 1; py < pixelHeight; py += 2)
                { // pixel y
                    bool below = false, above = pixelHeight - py < 2;
                    int startX = (py / 2) > sizeZ - 1 ? (py / 2) - sizeZ + 1 : 0,
                            startZ = (py / 2) > sizeZ - 1 ? sizeZ - 1 : (py / 2);
                    for (int vx = startX, vz = startZ;
                         vx <= sizeX && vz >= -1;
                         vx++, vz--)
                    { // vx is voxel x, vz is voxel z
                        if (!above && vz + 1 < sizeZ && vx < sizeX)
                        {
                            v = model.At(vx, vy, vz + 1);
                            if (v != 0)
                            {
                                renderer.RectRight(vy * scaleX, (py + 1) * scaleY, scaleX, scaleY, v);
                                above = true;
                            }
                        }
                        if (!below && vx > 0 && vz >= 0)
                        {
                            v = model.At(vx - 1, vy, vz);
                            if (v != 0)
                            {
                                renderer.RectVertical(vy * scaleX, py * scaleY, scaleX, scaleY, v);
                                below = true;
                            }
                        }
                        if ((above && below) || vx >= sizeX || vz < 0) break;
                        v = model.At(vx, vy, vz);
                        if (v != 0)
                        {
                            if (!above) renderer.RectVertical(vy * scaleX, (py + 1) * scaleY, scaleX, scaleY, v);
                            if (!below) renderer.RectRight(vy * scaleX, py * scaleY, scaleX, scaleY, v);
                            break;
                        }
                    }
                }
                // Finish main bulk of model
            }
        }

        public static uint IsoHeight(IModel model)
        {
            return (model.SizeZ * 2 +
                    Math.Max(model.SizeX, model.SizeY)
                    ) * 4;
        }

        public static uint IsoWidth(IModel model)
        {
            return (model.SizeX + model.SizeY) * 2;
            //- ((sizeVX + sizeVY & 1) << 2); // if sizeVX + sizeVY is odd, this is 4, otherwise it is 0
        }

        public static T PixelCubeIso<T>(this T renderer, IModel model) where T : ITriangleRenderer<T>
        {
            PixelCubeIso(model, renderer);
            return renderer;
        }

        public static void PixelCubeIso<T>(IModel model, T renderer) where T : ITriangleRenderer<T>
        {
            byte v;
            int sizeVX = (int)model.SizeX, sizeVY = (int)model.SizeY, sizeVZ = (int)model.SizeZ,
                    sizeVX2 = sizeVX * 2, sizeVY2 = sizeVY * 2,
                    pixelWidth = (int)IsoWidth(model);
            // To move one x+ in voxels is x + 2, y - 2 in pixels.
            // To move one x- in voxels is x - 2, y + 2 in pixels.
            // To move one y+ in voxels is x + 2, y + 2 in pixels.
            // To move one y- in voxels is x - 2, y - 2 in pixels.
            // To move one z+ in voxels is y + 4 in pixels.
            // To move one z- in voxels is y - 4 in pixels.
            for (int px = 0; px < pixelWidth; px += 4)
            {
                bool rightSide = px + 2 > sizeVY2, leftSide = !rightSide;
                int bottomPY = Math.Abs(sizeVX2 - 2 - px),
                        topPY = sizeVX2 - 2 + // black space at the bottom from the first column
                                (sizeVZ - 1) * 4 + // height of model
                                sizeVY2 - Math.Abs(sizeVY2 - 2 - px);

                // Begin drawing bottom row triangles
                if (px < sizeVX2 - 2)
                { // Left side of model
                    bool rightEmpty = true;
                    v = model.At(sizeVX - px / 2 - 2, 0, 0); // Front right
                    if (v != 0)
                    {
                        renderer.DrawRightTriangleLeftFace(px + 2, bottomPY - 4, v, sizeVX - px / 2 - 2, 0, 0);
                        renderer.DrawLeftTriangleLeftFace(px + 2, bottomPY - 6, v, sizeVX - px / 2 - 2, 0, 0);
                        rightEmpty = false;
                    }
                    v = model.At(sizeVX - px / 2 - 1, 0, 0); // Center
                    if (v != 0)
                    {
                        renderer.DrawLeftTriangleLeftFace(px, bottomPY - 4, v, sizeVX - px / 2 - 1, 0, 0);
                        if (rightEmpty)
                            renderer.DrawRightTriangleRightFace(px + 2, bottomPY - 4, v, sizeVX - px / 2 - 1, 0, 0);
                    }
                }
                else if (px > sizeVX2 - 2)
                { // Right side of model
                    bool leftEmpty = true;
                    v = model.At(0, px / 2 - sizeVX, 0); // Front left
                    if (v != 0)
                    {
                        renderer.DrawRightTriangleRightFace(px, bottomPY - 6, v, 0, px / 2 - sizeVX, 0);
                        renderer.DrawLeftTriangleRightFace(px, bottomPY - 4, v, 0, px / 2 - sizeVX, 0);
                        leftEmpty = false;
                    }
                    v = model.At(0, px / 2 - sizeVX + 1, 0); // Center
                    if (v != 0)
                    {
                        renderer.DrawRightTriangleRightFace(px + 2, bottomPY - 4, v, 0, px / 2 - sizeVX + 1, 0);
                        if (leftEmpty)
                            renderer.DrawLeftTriangleLeftFace(px, bottomPY - 4, v, 0, px / 2 - sizeVX + 1, 0);
                    }
                }
                else
                { // Very bottom
                    v = model.At(0, 0, 0);
                    if (v != 0)
                    {
                        renderer.DrawLeftTriangleLeftFace(px, bottomPY - 4, v, 0, 0, 0);
                        renderer.DrawRightTriangleRightFace(px + 2, bottomPY - 4, v, 0, 0, 0);
                        if (sizeVX % 2 == 0)
                            renderer.DrawRightTriangleRightFace(px, bottomPY - 6, v, 0, 0, 0);
                    }
                    else
                    {
                        v = model.At(px / 2 + 1, 0, 0);
                        if (v != 0)
                            renderer.DrawLeftTriangleRightFace(px, bottomPY - 4, v, px / 2 + 1, 0, 0);
                        v = model.At(0, px / 2 - sizeVX + 2, 0);
                        if (v != 0)
                            renderer.DrawRightTriangleLeftFace(px + 2, bottomPY - 4, v, 0, px / 2 - sizeVX + 2, 0);
                    }
                }
                // Finish drawing bottom row triangles

                // Begin drawing main bulk of model
                for (int py = bottomPY - 4; py <= topPY; py += 4)
                {
                    bool topSide = py > bottomPY + (sizeVZ - 1) * 4, bottomSide = !topSide;
                    int additive = (py - bottomPY) / 4 - sizeVZ + 1,
                            startVX = (px < sizeVX2 ? sizeVX - 1 - px / 2 : 0) + (topSide ? additive : 0),
                            startVY = (px < sizeVX2 ? 0 : px / 2 - sizeVX + 1) + (topSide ? additive : 0),
                            startVZ = bottomSide ? (py - bottomPY) / 4 : sizeVZ - 1;

                    bool left = false,
                            topLeft = false,
                            topRight = false,
                            right = false;
                    for (int vx = startVX, vy = startVY, vz = startVZ;
                         vx < sizeVX && vy < sizeVY && vz >= 0;
                         vx++, vy++, vz--)
                    {

                        // Order to check
                        // x, y-, z+ = Above front left
                        // x-, y, z+ = Above front right
                        // x, y, z+ = Above
                        // x, y-, z = Front left
                        // x-, y, z = Front right
                        // x, y, z  = Center
                        // x+, y, z = Back left
                        // x, y+, z = Back right
                        // x+, y, z- = Below back left
                        // x, y+ z- = Below back right

                        // OK here goes:
                        // x, y-, z+ = Above front left
                        if ((!left || !topLeft) && vx == 0 && vy > 0 && vz < sizeVZ - 1)
                        {
                            v = model.At(vx, vy - 1, vz + 1);
                            if (v != 0)
                            {
                                if (!topLeft)
                                {
                                    renderer.DrawLeftTriangleRightFace(px, py, v, vx, vy - 1, vz + 1);
                                    topLeft = true;
                                }
                                if (!left)
                                {
                                    renderer.DrawRightTriangleRightFace(px, py - 2, v, vx, vy - 1, vz + 1);
                                    left = true;
                                }
                            }
                        }

                        // x-, y, z+ = Above front right
                        if ((!topRight || !right) && vx > 0 && vy == 0 && vz < sizeVZ - 1)
                        {
                            v = model.At(vx - 1, vy, vz + 1);
                            if (v != 0)
                            {
                                if (!topRight)
                                {
                                    renderer.DrawRightTriangleLeftFace(px + 2, py, v, vx - 1, vy, vz + 1);
                                    topRight = true;
                                }
                                if (!right)
                                {
                                    renderer.DrawLeftTriangleLeftFace(px + 2, py - 2, v, vx - 1, vy, vz + 1);
                                    right = true;
                                }
                            }
                        }

                        // x, y, z+ = Above
                        if ((!topLeft || !topRight) && vz < sizeVZ - 1)
                        {
                            v = model.At(vx, vy, vz + 1);
                            if (v != 0)
                            {
                                if (!topLeft)
                                {
                                    renderer.DrawLeftTriangleLeftFace(px, py, v, vx, vy, vz + 1);
                                    topLeft = true;
                                }
                                if (!topRight)
                                {
                                    renderer.DrawRightTriangleRightFace(px + 2, py, v, vx, vy, vz + 1);
                                    topRight = true;
                                }
                            }
                        }

                        // x, y-, z = Front left
                        if (!left && vy > 0)
                        {
                            v = model.At(vx, vy - 1, vz);
                            if (v != 0)
                            {
                                renderer.DrawRightTriangleVerticalFace(px, py - 2, v, vx, vy - 1, vz);
                                left = true;
                            }
                        }

                        // x-, y, z = Front right
                        if (!right && vx > 0)
                        {
                            v = model.At(vx - 1, vy, vz);
                            if (v != 0)
                            {
                                renderer.DrawLeftTriangleVerticalFace(px + 2, py - 2, v, vx - 1, vy, vz);
                                right = true;
                            }
                        }

                        // x, y, z  = Center
                        if (left && topLeft && topRight && right) break;
                        v = model.At(vx, vy, vz);
                        if (v != 0)
                        {
                            if (!topLeft)
                                renderer.DrawLeftTriangleVerticalFace(px, py, v, vx, vy, vz);
                            if (!left)
                                renderer.DrawRightTriangleLeftFace(px, py - 2, v, vx, vy, vz);
                            if (!topRight)
                                renderer.DrawRightTriangleVerticalFace(px + 2, py, v, vx, vy, vz);
                            if (!right)
                                renderer.DrawLeftTriangleRightFace(px + 2, py - 2, v, vx, vy, vz);
                            break;
                        }

                        // x+, y, z = Back left
                        if ((!left || !topLeft) && vx < sizeVX - 1)
                        {
                            v = model.At(vx + 1, vy, vz);
                            if (v != 0)
                            {
                                if (!topLeft)
                                {
                                    renderer.DrawLeftTriangleRightFace(px, py, v, vx + 1, vy, vz);
                                    topLeft = true;
                                }
                                if (!left)
                                {
                                    renderer.DrawRightTriangleRightFace(px, py - 2, v, vx + 1, vy, vz);
                                    left = true;
                                }
                            }
                        }

                        // x, y+, z = Back right
                        if ((!right || !topRight) && vy < sizeVY - 1)
                        {
                            v = model.At(vx, vy + 1, vz);
                            if (v != 0)
                            {
                                if (!topRight)
                                {
                                    renderer.DrawRightTriangleLeftFace(px + 2, py, v, vx, vy + 1, vz);
                                    topRight = true;
                                }
                                if (!right)
                                {
                                    renderer.DrawLeftTriangleLeftFace(px + 2, py - 2, v, vx, vy + 1, vz);
                                    right = true;
                                }
                            }
                        }

                        // x+, y+ z = Back center
                        if ((!topLeft || !topRight) && vx < sizeVX - 1 && vy < sizeVY - 1)
                        {
                            v = model.At(vx + 1, vy + 1, vz);
                            if (v != 0)
                            {
                                if (!topRight)
                                {
                                    renderer.DrawRightTriangleRightFace(px + 2, py, v, vx + 1, vy + 1, vz);
                                    topRight = true;
                                }
                                if (!topLeft)
                                {
                                    renderer.DrawLeftTriangleLeftFace(px, py, v, vx + 1, vy + 1, vz);
                                    topLeft = true;
                                }
                            }
                        }

                        // x+, y, z- = Below back left
                        if (!left && vx < sizeVX - 1 && vz > 0)
                        {
                            v = model.At(vx + 1, vy, vz - 1);
                            if (v != 0)
                            {
                                renderer.DrawRightTriangleVerticalFace(px, py - 2, v, vx + 1, vy, vz - 1);
                                left = true;
                            }
                        }

                        // x, y+ z- = Below back right
                        if (!right && vy < sizeVY - 1 && vz > 0)
                        {
                            v = model.At(vx, vy + 1, vz - 1);
                            if (v != 0)
                            {
                                renderer.DrawLeftTriangleVerticalFace(px + 2, py - 2, v, vx, vy + 1, vz - 1);
                                right = true;
                            }
                        }

                        // Debugging
                        //                    if (startVX == 10 && startVY == 0 && startVZ == 3) {
                        //                        Gdx.app.log("debug", "Coord: " + vx + ", " + vy + ", " + vz);
                        ////                    if (!topLeft)
                        //                        renderer.drawLeftTriangle(px, py, flash());
                        ////                    if (!left)
                        //                        renderer.drawRightTriangle(px, py - 2, flash());
                        ////                    if (!topRight)
                        //                        renderer.drawRightTriangle(px + 2, py, flash());
                        ////                    if (!right)
                        //                        renderer.drawLeftTriangle(px + 2, py - 2, flash());
                        //                    }
                        // Finish debugging
                    }
                }
                // Finish drawing main bulk of model

                // Begin drawing top triangles
                if (px + 2 < sizeVY2)
                { // Top left triangles
                    v = model.At(sizeVX - 1, px / 2 + 1, sizeVZ - 1);
                    if (v != 0)
                        renderer.DrawLeftTriangleVerticalFace(px + 2, topPY, v, sizeVX - 1, px / 2 + 1, sizeVZ - 1);
                }
                else if (px + 2 > sizeVY2)
                { // Top right triangles
                    v = model.At(sizeVY - 1 + sizeVX - px / 2, sizeVY - 1, sizeVZ - 1);
                    if (v != 0)
                        renderer.DrawRightTriangleVerticalFace(px, topPY, v, sizeVY - 1 + sizeVX - px / 2, sizeVY - 1, sizeVZ - 1);
                }
                // Finish drawing top triangles.

                // Drawing right edge (only for when sizeVX + sizeVY is odd numbered)
                if ((sizeVX + sizeVY) % 2 == 1)
                {
                    int vx = 0, vy = sizeVY - 1,
                            bottom = Math.Abs(sizeVX2 - 2 - pixelWidth);
                    v = model.At(vx, vy, 0);
                    if (v != 0)
                        renderer.DrawRightTriangleRightFace(pixelWidth + 2, bottom - 4, v, vx, vy, 0); // lower right corner
                    for (int py = bottom; py < bottom + sizeVZ * 4; py += 4)
                    {
                        int vz = (py - bottom) / 4;
                        bool aboveEmpty = true;
                        if (vz != sizeVZ - 1)
                        {
                            v = model.At(vx, vy, vz + 1);
                            if (v != 0)
                            {
                                renderer.DrawRightTriangleRightFace(pixelWidth + 2, py, v, vx, vy, vz);
                                aboveEmpty = false;
                            }
                        }
                        v = model.At(vx, vy, vz);
                        if (v != 0)
                        {
                            renderer.DrawLeftTriangleRightFace(pixelWidth + 2, py - 2, v, vx, vy, vz);
                            if (aboveEmpty)
                                renderer.DrawRightTriangleVerticalFace(pixelWidth + 2, py, v, vx, vy, vz);
                        }
                    }
                }
                // Finish drawing right edge
            }
        }

        public static T PixelCubeIso<T>(this T renderer, VoxelSeq seq) where T : ITriangleRenderer<T>
        {
            PixelCubeIso(seq, renderer);
            return renderer;
        }

        public static void PixelCubeIso<T>(VoxelSeq seq, T renderer) where T : ITriangleRenderer<T>
        {
            int len = seq.Order.Count;
            ulong sizeX = seq.SizeX, sizeY = seq.SizeY, sizeZ = seq.SizeZ,
                     pixelWidth = (sizeY + sizeX) * 2 + 5,
                     pixelHeight = (sizeX + sizeY + sizeZ * 2) * 2 + 5;
            seq.Order.Sort(VoxelSeq.Side45[seq.Rotation]);
            for (int i = 0; i < len; i++)
            {
                byte v = seq.GetAtHollow(i);
                if (v != 0)
                {
                    ulong xyz = seq.KeyAtRotatedHollow(i),
                             x = VoxelSeq.ExtractX(xyz),
                             y = VoxelSeq.ExtractY(xyz),
                             z = VoxelSeq.ExtractZ(xyz),
                             xPos = (sizeY - y + x) * 2 + 1,
                             yPos = (z * 2 + sizeX + sizeY - x - y) * 2 + 1,
                             dep = 3 * (x + y + z) + 256;
                    renderer.DrawLeftTriangleLeftFace((int)xPos, (int)yPos, v, (int)dep, (int)x, (int)y, (int)z);
                    renderer.DrawRightTriangleLeftFace((int)xPos, (int)yPos + 2, v, (int)dep, (int)x, (int)y, (int)z);
                    renderer.DrawLeftTriangleRightFace((int)xPos + 2, (int)yPos + 2, v, (int)dep, (int)x, (int)y, (int)z);
                    renderer.DrawRightTriangleRightFace((int)xPos + 2, (int)yPos, v, (int)dep, (int)x, (int)y, (int)z);
                    //if (z >= sizeZ - 1 || seq.getRotated(x, y, z + 1) == 0)
                    renderer.DrawLeftTriangleVerticalFace((int)xPos, (int)yPos + 4, v, (int)dep, (int)x, (int)y, (int)z);
                    renderer.DrawRightTriangleVerticalFace((int)xPos + 2, (int)yPos + 4, v, (int)dep, (int)x, (int)y, (int)z);
                }
            }
        }
    }
}
