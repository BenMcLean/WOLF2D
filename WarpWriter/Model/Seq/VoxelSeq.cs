using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WarpWriter.Model.Fetch;

namespace WarpWriter.Model.Seq
{
    public class VoxelSeq
    {
        public VoxelSeq() : this(64) { }
        public VoxelSeq(int capacity)
        {
            Voxels = new Dictionary<ulong, byte>(capacity);
            Full = new List<ulong>(capacity);
            Order = new List<ulong>(capacity);
            SizeX = 64UL;
            SizeY = 64UL;
            SizeZ = 64UL;
        }
        public ulong SizeX { get; set; }
        public ulong SizeY { get; set; }
        public ulong SizeZ { get; set; }
        public int Rotation { get; set; }
        public Dictionary<ulong, byte> Voxels { get; set; }
        public List<ulong> Full { get; set; }
        public List<ulong> Order { get; set; }

        public VoxelSeq PutArray(byte[][][] voxels)
        {
            ulong sizeX = (ulong)(voxels.Length);
            ulong sizeY = (ulong)(voxels[0].Length);
            ulong sizeZ = (ulong)(voxels[0][0].Length);
            SizeX = SizeY = SizeZ = Math.Max(sizeX, Math.Max(sizeY, sizeZ));
            for (ulong x = 0; x < sizeX; x++)
            {
                for (ulong y = 0; y < sizeY; y++)
                {
                    for (ulong z = 0; z < sizeZ; z++)
                    {
                        if (voxels[x][y][z] != 0)
                            Add(x, y, z, voxels[x][y][z]);
                    }
                }
            }
            return this;
        }

        public VoxelSeq PutSurface(byte[][][] voxels)
        {
            ulong sizeX = (ulong)(voxels.Length);
            ulong sizeY = (ulong)(voxels[0].Length);
            ulong sizeZ = (ulong)(voxels[0][0].Length);
            SizeX = SizeY = SizeZ = Math.Max(sizeX, Math.Max(sizeY, sizeZ));
            for (ulong y = 0; y < sizeY; y++)
            {
                for (ulong z = 0; z < sizeZ; z++)
                {
                    if (voxels[0][y][z] != 0)
                        Add(0, y, z, voxels[0][y][z]);
                    if (voxels[sizeX - 1][y][z] != 0)
                        Add(sizeX - 1, y, z, voxels[sizeX - 1][y][z]);
                }
            }
            for (ulong x = 1UL; x < sizeX - 1UL; x++)
            {
                for (ulong z = 0; z < sizeZ; z++)
                {
                    if (voxels[x][0][z] != 0)
                        Add(x, 0, z, voxels[x][0][z]);
                    if (voxels[x][sizeY - 1][z] != 0)
                        Add(x, sizeY - 1, z, voxels[x][sizeY - 1][z]);
                }
                for (ulong y = 1UL; y < sizeY - 1UL; y++)
                {
                    if (voxels[x][y][0] != 0)
                        Add(x, y, 0, voxels[x][y][0]);
                    if (voxels[x][y][sizeZ - 1] != 0)
                        Add(x, y, sizeZ - 1, voxels[x][y][sizeZ - 1]);
                    for (ulong z = 1; z < sizeZ - 1UL; z++)
                    {
                        if (voxels[x][y][z] != 0 && (voxels[x - 1][y][z] == 0 || voxels[x + 1][y][z] == 0 ||
                                voxels[x][y - 1][z] == 0 || voxels[x][y + 1][z] == 0 ||
                                voxels[x][y][z - 1] == 0 || voxels[x][y][z + 1] == 0))
                            Add(x, y, z, voxels[x][y][z]);
                    }
                }
            }
            Order.Clear();
            Order.AddRange(Full);
            return this;
        }

        /**
         * Puts all non-empty voxels in {@code model} into this VoxelSeq, and also sets up the voxels touching empty space
         * so they will be iterated through by some of the rendering methods, instead of needing to iterate over all voxels.
         * @param model an IModel; the highest of its {@link IModel#sizeX()}, {@link IModel#sizeY()} and
         *              {@link IModel#sizeZ()} will be used for all sizes to allow rotation.
         */
        public VoxelSeq PutModel(IModel model)
        {
            ulong sizeX = (ulong)(model.SizeX);
            ulong sizeY = (ulong)(model.SizeY);
            ulong sizeZ = (ulong)(model.SizeZ);
            SizeX = SizeY = SizeZ = Math.Max(sizeX, Math.Max(sizeY, sizeZ));
            byte v;
            for (ulong x = 0; x < sizeX; x++)
            {
                for (ulong y = 0; y < sizeY; y++)
                {
                    for (ulong z = 0; z < sizeZ; z++)
                    {
                        v = model.At((int)x, (int)y, (int)z);
                        if (v != 0)
                            Add(x, y, z, v);
                    }
                }
            }
            return Hollow();
        }
        /**
         * Resets the order of potentially-visible voxels as used by the rotation-related methods; since this order is not
         * changed by normal {@link #Add(int, byte)} and {@link #remove(int)}, this method must be used to complete any
         * changes to the structure of the VoxelSeq.
         */
        public VoxelSeq Hollow()
        {
            Order.Clear();
            int sz = Full.Count;
            ulong k, x, y, z;
            for (int i = 0; i < sz; i++)
            {
                k = Full[i];
                x = ExtractX(k);
                y = ExtractY(k);
                z = ExtractZ(k);
                if (x <= 0 || x >= SizeX - 1 || y <= 0 || y >= SizeY - 1 || z <= 0 || z >= SizeZ - 1 ||
                        !Voxels.ContainsKey(Fuse(x - 1, y, z)) || !Voxels.ContainsKey(Fuse(x + 1, y, z)) ||
                        !Voxels.ContainsKey(Fuse(x, y - 1, z)) || !Voxels.ContainsKey(Fuse(x, y + 1, z)) ||
                        !Voxels.ContainsKey(Fuse(x, y, z - 1)) || !Voxels.ContainsKey(Fuse(x, y, z + 1)))
                    Order.Add(k);
            }
            return this;
        }

        public void Add(ulong x, ulong y, ulong z, byte c)
        {
            ulong f = Fuse(x, y, z);
            if (Voxels.ContainsKey(f))
            {
                Voxels[f] = c;
            }
            else
            {
                Voxels.Add(f, c);
                Full.Add(f);
            }
        }


        /**
 * Combines 3 ulong components x, y, and z, each between 0 and 2097151 inclusive, into one ulong that can be used as a key
 * in this HashMap3D. 63 of the 64 bits in the returned ulong have the potential to be used, allowing about nine million
 * million million (9223372036854775808) possible keys that never produce garbage or need garbage collection (at least
 * for themselves).
 *
 * @param x the x component, between 0 and 2097151; this can be extracted with {@link #extractX(ulong)}
 * @param y the y component, between 0 and 2097151; this can be extracted with {@link #extractY(ulong)}
 * @param z the z component, between 0 and 2097151; this can be extracted with {@link #extractZ(ulong)}
 * @return a fused XYZ index that can be used as one key; will be unique for any (x,y,z) triple within range
 */
        public static ulong Fuse(ulong x, ulong y, ulong z)
        {
            return (z & 0x1FFFFFUL) << 42 | (y & 0x1FFFFFUL) << 21 | (x & 0x1FFFFFUL);
        }

        /**
         * Given a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}, this gets the x component back out of it.
         *
         * @param fused a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}
         * @return the x component stored in fused
         */
        public static ulong ExtractX(ulong fused)
        {
            return fused & 0x1FFFFFUL;
        }

        /**
         * Given a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}, this gets the y component back out of it.
         *
         * @param fused a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}
         * @return the y component stored in fused
         */
        public static ulong ExtractY(ulong fused)
        {
            return fused >> 21 & 0x1FFFFFUL;
        }

        /**
         * Given a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}, this gets the z component back out of it.
         *
         * @param fused a fused XYZ index as produced by {@link #fuse(ulong, ulong, ulong)}
         * @return the z component stored in fused
         */
        public static ulong ExtractZ(ulong fused)
        {
            return fused >> 42 & 0x1FFFFFUL;
        }

        public ulong Rotate(ulong k, int rotation)
        {
            switch (rotation)
            {
                // 0-3 have z pointing towards z+ and the voxels rotating on that axis
                case 0: return k;
                case 1: return (k & 0x7FFFFC0000000000UL) | SizeX - (k & 0x00000000001FFFFFUL) << 21 | (k >> 21 & 0x00000000001FFFFFUL);
                case 2: return (k & 0x7FFFFC0000000000UL) | (SizeY << 21) - (k & 0x000003FFFFE00000UL) | SizeX - (k & 0x00000000001FFFFFUL);
                case 3: return (k & 0x7FFFFC0000000000UL) | (k & 0x00000000001FFFFFUL) << 21 | (SizeY - (k >> 21 & 0x00000000001FFFFFUL));
                // 4-7 have z pointing towards y+ and the voxels rotating on that axis
                case 4: return (k >> 21 & 0x000003FFFFE00000UL) | (SizeY << 21) - (k & 0x000003FFFFE00000UL) << 21 | (k & 0x00000000001FFFFFUL);
                case 5: return (k >> 21 & 0x000003FFFFE00000UL) | (k & 0x00000000001FFFFFUL) << 42 | (k >> 21 & 0x00000000001FFFFFUL);
                case 6: return (k >> 21 & 0x000003FFFFE00000UL) | (k & 0x000003FFFFE00000UL) << 21 | SizeX - (k & 0x00000000001FFFFFUL);
                case 7: return (k >> 21 & 0x000003FFFFE00000UL) | (SizeX - (k & 0x00000000001FFFFFUL) << 42) | SizeY - (k >> 21 & 0x00000000001FFFFFUL);
                // 8-11 have z pointing towards z-
                case 8: return (SizeZ << 42) - (k & 0x7FFFFC0000000000UL) | (k & 0x000003FFFFE00000UL) | (k & 0x00000000001FFFFFUL);
                case 9: return (SizeZ << 42) - (k & 0x7FFFFC0000000000UL) | (SizeY) - (k >> 21 & 0x00000000001FFFFFUL) | (k & 0x00000000001FFFFFUL) << 21;
                case 10: return (SizeZ << 42) - (k & 0x7FFFFC0000000000UL) | (SizeY << 21) - (k & 0x000003FFFFE00000UL) | SizeX - (k & 0x00000000001FFFFFUL);
                case 11: return (SizeZ << 42) - (k & 0x7FFFFC0000000000UL) | (k >> 21 & 0x00000000001FFFFFUL) | SizeX - (k & 0x00000000001FFFFFUL) << 21;
                // 12-15 have z pointing towards y-
                case 12: return (SizeZ << 21) - (k >> 21 & 0x000003FFFFE00000UL) | (k & 0x000003FFFFE00000UL) << 21 | (k & 0x00000000001FFFFFUL);
                case 13: return (SizeZ << 21) - (k >> 21 & 0x000003FFFFE00000UL) | SizeX - (k & 0x00000000001FFFFFUL) << 42 | (k >> 21 & 0x00000000001FFFFFUL);
                case 14: return (SizeZ << 21) - (k >> 21 & 0x000003FFFFE00000UL) | (SizeY << 42) - (k << 21 & 0x7FFFFC0000000000UL) | SizeX - (k & 0x00000000001FFFFFUL);
                case 15: return (SizeZ << 21) - (k >> 21 & 0x000003FFFFE00000UL) | (k & 0x00000000001FFFFFUL) << 42 | SizeY - (k >> 21 & 0x00000000001FFFFFUL);
                // 16-19 have z pointing towards x+ and the voxels rotating on that axis
                case 16: return (k >> 42 & 0x00000000001FFFFFUL) | (k & 0x000003FFFFE00000UL) | (k << 42 & 0x7FFFFC0000000000UL);
                case 17: return (k >> 42 & 0x00000000001FFFFFUL) | (k << 21 & 0x7FFFFC0000000000UL) | (SizeX - (k & 0x00000000001FFFFFUL) << 21);
                case 18: return (k >> 42 & 0x00000000001FFFFFUL) | (SizeY << 21) - (k & 0x000003FFFFE00000UL) | (SizeX - (k & 0x00000000001FFFFFUL)) << 42;
                case 19: return (k >> 42 & 0x00000000001FFFFFUL) | (SizeY << 42) - (k << 21 & 0x7FFFFC0000000000UL) | (k << 21 & 0x000003FFFFE00000UL);
                // 20-23 have z pointing towards x- and the voxels rotating on that axis
                case 20: return SizeZ - (k >> 42 & 0x00000000001FFFFFUL) | (k & 0x000003FFFFE00000UL) | (k << 42 & 0x7FFFFC0000000000UL);
                case 21: return SizeZ - (k >> 42 & 0x00000000001FFFFFUL) | (k << 21 & 0x7FFFFC0000000000UL) | (SizeX - (k & 0x00000000001FFFFFUL) << 21);
                case 22: return SizeZ - (k >> 42 & 0x00000000001FFFFFUL) | (SizeY << 21) - (k & 0x000003FFFFE00000UL) | (SizeX - (k & 0x00000000001FFFFFUL)) << 42;
                case 23: return SizeZ - (k >> 42 & 0x00000000001FFFFFUL) | (SizeY << 42) - (k << 21 & 0x7FFFFC0000000000UL) | (k << 21 & 0x000003FFFFE00000UL);
                default:
                    throw new ArgumentException("This shouldn't be happening! The rotation " + rotation + " was bad.");
                    //                    return 0;
            }
        }
        public VoxelSeq CounterX()
        {
            int r = Rotation;
            switch (r & 28)
            { // 16, 8, 4
                case 0:
                case 8:
                    Rotation = (r ^ 4);
                    break;
                case 12:
                case 4:
                    Rotation = (r ^ 12);
                    break;
                case 16:
                    Rotation = ((r + 1 & 3) | 16);
                    break;
                case 20:
                    Rotation = ((r - 1 & 3) | 20);
                    break;
            }
            return this;
        }

        public VoxelSeq CounterY()
        {
            int r = Rotation;
            switch (r & 28) // 16, 8, and 4 can each be set.
            {
                case 0:
                    Rotation = ((r & 3) | 20);
                    break;
                case 4:
                    Rotation = ((r - 1 & 3) | (r & 12));
                    break;
                case 8:
                    Rotation = ((2 - r & 3) | 16);
                    break;
                case 12:
                    Rotation = ((r + 1 & 3) | (r & 12));
                    break;
                case 16:
                    Rotation = (-r & 3);
                    break;
                case 20:
                    Rotation = ((2 + r & 3) | 8);
                    break;
            }
            return this;
        }

        public VoxelSeq CounterZ()
        {
            Rotation = ((Rotation - 1 & 3) | (Rotation & 28));
            return this;
        }
        public VoxelSeq ClockX()
        {
            int r = Rotation;
            switch (r & 28)
            {
                case 4:
                case 12:
                    Rotation = (r ^ 4);
                    break;
                case 0:
                case 8:
                    Rotation = (r ^ 12);
                    break;
                case 16:
                    Rotation = ((r - 1 & 3) | 16);
                    break;
                case 20:
                    Rotation = ((r + 1 & 3) | 20);
                    break;
            }
            return this;
        }

        public VoxelSeq ClockY()
        {
            int r = Rotation;
            switch (r & 28) // 16, 8, and 4 can each be set.
            {
                case 0:
                    Rotation = ((-r & 3) | 16);
                    break;
                case 4:
                    Rotation = ((r + 1 & 3) | (r & 12));
                    break;
                case 8:
                    Rotation = ((2 + r & 3) | 20);
                    break;
                case 12:
                    Rotation = ((r - 1 & 3) | (r & 12));
                    break;
                case 16:
                    Rotation = ((2 - r & 3) | 8);
                    break;
                case 20:
                    Rotation = (r & 3);
                    break;
            }
            return this;
        }

        public VoxelSeq ClockZ()
        {
            Rotation = ((Rotation + 1 & 3) | (Rotation & 28));
            return this;
        }

        public VoxelSeq Reset()
        {
            Rotation = (0);
            return this;
        }
        public ulong KeyAtRotatedFull(int idx)
        {
            if (idx < 0 || idx >= Full.Count)
                return 0;
            return Rotate(Full[idx], Rotation);
        }
        public byte GetAtFull(int idx)
        {
            if (idx < 0 || idx >= Full.Count)
                return 0;
            return Voxels[Full[idx]];
        }

        public ulong KeyAtRotatedHollow(int idx)
        {
            if (idx < 0 || idx >= Order.Count)
                return 0;
            return Rotate(Order[idx], Rotation);
        }
        public byte GetAtHollow(int idx)
        {
            if (idx < 0 || idx >= Order.Count)
                return 0;
            return Voxels[Order[idx]];
        }
        public byte GetRotated(ulong x, ulong y, ulong z)
        {
            Voxels.TryGetValue(Rotate(Fuse(x, y, z), ((-Rotation) & 3) | Rotation & 0xFC), out byte v);
            return v;
        }
        public static readonly Comparison<ulong>[] Side = new Comparison<ulong>[]
        {
            //0
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than z, and y is irrelevant
                    ((left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL) + (left >> 42) - (right >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y as 1024 times more important than z, and x is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + (left >> 42) - (right >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than z, and y is irrelevant)), reversed for x
                    ((right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL) + (left >> 42) - (right >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y as 1024 times more important than z, and x is irrelevant)), reversed for y
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + (left >> 42) - (right >> 42))),
            //4
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than reversed y, and z is irrelevant
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL) << 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y as 1024 times more important than x, and z is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x as 1024 times more important than y, and z is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL) << 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed y as 1024 times more important than reversed x, and z is irrelevant
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL)))),
            //8
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than reversed z, and y is irrelevant
                    ((left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL) + (right >> 42) - (left >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed y as 1024 times more important than reversed z, and x is irrelevant
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + (right >> 42) - (left >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than reversed z, and y is irrelevant)), reversed for x
                    ((right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL) + (right >> 42) - (left >> 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y as 1024 times more important than reversed z, and x is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + (right >> 42) - (left >> 42))),
            //12
            (ulong left, ulong right) => Math.Sign((long)
                    // values x as 1024 times more important than y, and z is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL) << 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y as 1024 times more important than reversed x, and z is irrelevant
                    ((left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x as 1024 times more important than reversed y, and z is irrelevant
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL) << 42))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed y as 1024 times more important than x, and z is irrelevant
                    ((right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL)))),
            //16
            (ulong left, ulong right) => Math.Sign((long)
                    // values z as many times more important than x, and y is irrelevant
                    (((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values z as 1024 times more important than y, and x is irrelevant
                    (((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)) + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values z as many times more important than reversed x, and y is irrelevant
                    (((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values z as many times more important than reversed y, and x is irrelevant
                    (((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)) + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL))),
            //42
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z as many times more important than x, and y is irrelevant
                    (((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)) + ((left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z as 1024 times more important than y, and x is irrelevant
                    (((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)) + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z as many times more important than reversed x, and y is irrelevant
                    (((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)) + ((right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL)))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z as many times more important than reversed y, and x is irrelevant
                    (((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)) + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)))
        };
        public static readonly Comparison<ulong>[] Side45 = new Comparison<ulong>[]
        {
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and y equally, either as 1024 times more important than z
                    ((left >> 42) - (right >> 42)
                            + (left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL)
                            + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and y equally, either as 1024 times more important than z
                    ((left >> 42) - (right >> 42)
                            + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL)
                            + (right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and reversed y equally, either as 1024 times more important than z
                    ((left >> 42) - (right >> 42)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and reversed y equally, either as 1024 times more important than z
                    ((left >> 42) - (right >> 42)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and z equally, either as 1024 times more important than reversed y
                    ((left << 42 & 0x7FFFFC0000000000UL) - (right << 42 & 0x7FFFFC0000000000UL) + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) +
                            (left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y and z equally, either as many times more important than x
                    ((left << 21 & 0x7FFFFC0000000000UL) - (right << 21 & 0x7FFFFC0000000000UL) + (left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL) +
                            (left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and z equally, either as 1024 times more important than y
                    ((right << 42 & 0x7FFFFC0000000000UL) - (left << 42 & 0x7FFFFC0000000000UL) + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) +
                            (left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed y and z equally, either as many times more important than reversed x
                    ((right << 21 & 0x7FFFFC0000000000UL) - (left << 21 & 0x7FFFFC0000000000UL) + (right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL) +
                            (left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and y equally, either as 1024 times more important than reversed z
                    ((right >> 42) - (left >> 42) +
                            (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) +
                            (left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and reversed y equally, either as 1024 times more important than reversed z
                    ((right >> 42) - (left >> 42)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (left << 21 & 0x000003FFFFE00000UL) - (right << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and reversed y equally, either as 1024 times more important than reversed z
                    ((right >> 42) - (left >> 42)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and y equally, either as 1024 times more important than reversed z
                    ((right >> 42) - (left >> 42)
                            + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL)
                            + (right << 21 & 0x000003FFFFE00000UL) - (left << 21 & 0x000003FFFFE00000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values y and reversed z equally, either as many times more important than x
                    ((left << 21 & 0x7FFFFC0000000000UL) - (right << 21 & 0x7FFFFC0000000000UL) +
                            (left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL) +
                            (right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and reversed z equally, either as 1024 times more important than y
                    ((right << 42 & 0x7FFFFC0000000000UL) - (left << 42 & 0x7FFFFC0000000000UL) +
                            (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL) +
                            (right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed y and reversed z equally, either as many times more important than reversed x
                    ((right << 21 & 0x7FFFFC0000000000UL) - (left << 21 & 0x7FFFFC0000000000UL) +
                            (right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL) +
                            (right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and reversed z equally, either as 1024 times more important than reversed y
                    ((left << 42 & 0x7FFFFC0000000000UL) - (right << 42 & 0x7FFFFC0000000000UL) +
                            (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL) +
                            (right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values z and y equally, either as many times more important than x
                    ((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)
                            + (left << 21 & 0x7FFFFC0000000000UL) - (right << 21 & 0x7FFFFC0000000000UL)
                            + (left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and z equally, either as 1024 times more important than y
                    ((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)
                            + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL)
                            + (right << 42 & 0x7FFFFC0000000000UL) - (left << 42 & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values z and reversed y equally, either as many times more important than reversed x
                    ((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)
                            + (right << 21 & 0x7FFFFC0000000000UL) - (left << 21 & 0x7FFFFC0000000000UL)
                            + (right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and z equally, either as 1024 times more important than reversed y
                    ((left & 0x7FFFFC0000000000UL) - (right & 0x7FFFFC0000000000UL)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (left << 42 & 0x7FFFFC0000000000UL) - (right << 42 & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z and reversed y equally, either as many times more important than x
                    ((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)
                            + (right << 21 & 0x7FFFFC0000000000UL) - (left << 21 & 0x7FFFFC0000000000UL)
                            + (left & 0x00000000001FFFFFUL) - (right & 0x00000000001FFFFFUL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values x and reversed z equally, either as 1024 times more important than y
                    ((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)
                            + (left & 0x000003FFFFE00000UL) - (right & 0x000003FFFFE00000UL)
                            + (left << 42 & 0x7FFFFC0000000000UL) - (right << 42 & 0x7FFFFC0000000000UL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed z and y equally, either as many times more important than reversed x
                    ((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)
                            + (left << 21 & 0x7FFFFC0000000000UL) - (right << 21 & 0x7FFFFC0000000000UL)
                            + (right & 0x00000000001FFFFFUL) - (left & 0x00000000001FFFFFUL))),
            (ulong left, ulong right) => Math.Sign((long)
                    // values reversed x and reversed z equally, either as 1024 times more important than reversed y
                    ((right & 0x7FFFFC0000000000UL) - (left & 0x7FFFFC0000000000UL)
                            + (right & 0x000003FFFFE00000UL) - (left & 0x000003FFFFE00000UL)
                            + (right << 42 & 0x7FFFFC0000000000UL) - (left << 42 & 0x7FFFFC0000000000UL)))

        };
    }
}
