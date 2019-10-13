using WarpWriter.Model.Decide;

namespace WarpWriter.Model.Fetch
{
    public abstract class Fetch : IFetch, IDecide
    {
        public abstract byte At(int x, int y, int z);

        public byte At(int y, int z)
        {
            return At(0, y, z);
        }

        public byte At(int z)
        {
            return At(0, z);
        }

        public bool Bool(int x, int y, int z)
        {
            return At(x, y, z) != 0;
        }

        public bool Bool(int y, int z)
        {
            return Bool(0, y, z);
        }

        public bool Bool(int z)
        {
            return Bool(0, z);
        }

        public Fetch Next { get; set; }

        public Fetch Add(Fetch addFetch)
        {
            Fetch current, next = this;
            do
            {
                current = next;
                next = current.Next;
            } while (next != null);
            current.Next = addFetch;
            return this;
        }
    }
}
