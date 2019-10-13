using System;

namespace WarpWriter.Model.Extra
{
    public class RNG
    {
        public ulong State { get; set; }
        /**
         * Default constructor; uses a fixed seed of 1.
         */
        public RNG() : this(1UL)
        {

        }

        /**
         * Constructs a RNG with the given seed as-is; any seed can be given.
         * @param seed any ulong
         */
        public RNG(ulong seed)
        {
            State = seed;
        }

        /**
         * Get up to 32 bits (inclusive) of random output; the int this produces
         * will not require more than {@code bits} bits to represent.
         *
         * @param bits an int between 1 and 32, both inclusive
         * @return a random number that fits in the specified number of bits
         */

        public int Next(int bits)
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return (int)(z ^ (z >> 31)) >> (32 - bits);
        }

        /**
         * Get a random integer between Integer.MIN_VALUE to Integer.MAX_VALUE (both inclusive).
         *
         * @return a 32-bit random int.
         */

        public uint NextUInt()
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return (uint)(z ^ (z >> 31));
        }

        /**
         * Get a random ulong between ulong.MIN_VALUE to ulong.MAX_VALUE (both inclusive).
         *
         * @return a 64-bit random ulong.
         */

        public ulong NextULong()
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }
        /**
         * Get a random integer between Integer.MIN_VALUE to Integer.MAX_VALUE (both inclusive), but advances the state
         * "backwards," such that calling {@link #nextInt()} alternating with this method will return the same pair of
         * numbers for as ulong as you keep alternating those two calls. This can be useful with {@link #skip(ulong)} when it
         * advances ahead by a large amount and you want to step backward to reverse another set of forward-advancing number
         * generations that had been done by other code.
         *
         * @return a 32-bit random int.
         */
        public uint PreviousUInt()
        {
            ulong z = State -= 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return (uint)(z ^ (z >> 31));
        }

        /**
         * Get a random ulong between ulong.MIN_VALUE to ulong.MAX_VALUE (both inclusive), but advances the state
         * "backwards," such that calling {@link #nextLong()} alternating with this method will return the same pair of
         * numbers for as ulong as you keep alternating those two calls. This can be useful with {@link #skip(ulong)} when it
         * advances ahead by a large amount and you want to step backward to reverse another set of forward-advancing number
         * generations that had been done by other code.
         *
         * @return a 64-bit random ulong.
         */
        public ulong PreviousULong()
        {
            ulong z = State -= 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return (z ^ (z >> 31));
        }


        /**
         * Get a random bit of state, interpreted as true or false with approximately equal likelihood.
         * <br>
         * This implementation uses a sign check and is able to avoid some calculations needed to get a full int or ulong.
         *
         * @return a random boolean.
         */

        public bool NextBool()
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            return ((z ^ (z >> 27)) * 0x94D049BB133111EBUL) < 0;
        }

        /**
         * Gets a random double between 0.0 inclusive and 1.0 exclusive.
         * This returns a maximum of 0.9999999999999999 because that is the largest double value that is less than 1.0 .
         *
         * @return a double between 0.0 (inclusive) and 0.9999999999999999 (inclusive)
         */

        public double NextDouble()
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return ((z ^ (z >> 31)) & 0x1fffffffffffffUL) * 1.1102230246251565E-16;
        }

        /**
         * Gets a random float between 0.0f inclusive and 1.0f exclusive.
         * This returns a maximum of 0.99999994 because that is the largest float value that is less than 1.0f .
         *
         * @return a float between 0f (inclusive) and 0.99999994f (inclusive)
         */

        public float NextFloat()
        {
            ulong z = State += 0x9E3779B97F4A7C15UL;
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return ((z ^ (z >> 31)) & 0xffffffUL) * 5.9604645E-8f;
        }

        /**
         * Creates a copy of this RNG; it will generate the same random numbers, given the same calls in order, as
         * this RNG at the point copy() is called. The copy will not share references with this RNG.
         * @return a copy of this IRNG
         */

        public RNG Copy()
        {
            return new RNG(State);
        }

        /**
         * Advances or rolls back the SkippingRandomness' state without actually generating each number. Skips forward
         * or backward a number of steps specified by advance, where a step is equal to one call to {@link #nextLong()},
         * and returns the random number produced at that step. Negative numbers can be used to step backward, or 0 can be
         * given to get the most-recently-generated ulong from {@link #nextLong()}.
         *
         * @param advance Number of future generations to skip over; can be negative to backtrack, 0 gets the most-recently-generated number
         * @return the random ulong generated after skipping forward or backwards by {@code advance} numbers
         */

        public ulong Skip(ulong advance)
        {
            ulong z = (State += 0x9E3779B97F4A7C15UL * advance);
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
            return z ^ (z >> 31);
        }



        public override String ToString()
        {
            return "RNG with state " + State;
        }

        /**
         * Given the output of a call to {@link #nextLong()} as {@code out}, this finds the state of the RNG that
         * produce that output. If you set the state of a RNG with {@link #setState(ulong)} to the result of this
         * method and then call {@link #nextLong()} on it, you should get back {@code out}.
         * <br>
         * This isn't as fast as {@link #nextLong()}, but both run in constant time. Some random number generators take more
         * than constant time to reverse, so one was chosen for this class that would still be efficient ({@link LightRNG}).
         * <br>
         * This will not necessarily work if out was produced by a generator other than a RNG, or if it was produced
         * with the bounded {@link #nextLong(ulong)} method by any generator.
         * @param out a ulong as produced by {@link #nextLong()}, without changes
         * @return the state of the RNG that will produce the given ulong
         */
        public static ulong InverseNextULong(ulong output)
        {
            output ^= output >> 31 ^ output >> 62;
            output *= 0x319642B2D24D8EC3UL;
            output ^= output >> 27 ^ output >> 54;
            output *= 0x96DE1B173F119089UL;
            return (output ^ output >> 30 ^ output >> 60) - 0x9E3779B97F4A7C15UL;
        }

        /**
         * Returns the number of steps (where a step is equal to one call to most random number methods in this class)
         * needed to go from receiving out1 from a RNG's {@link #nextLong()} method to receiving out2 from another
         * call. This number can be used with {@link #skip(ulong)} to move a RNG forward or backward by the desired
         * distance.
         * @param out1 a ulong as produced by {@link #nextLong()}, without changes
         * @param out2 a ulong as produced by {@link #nextLong()}, without changes
         * @return the number of calls to {@link #nextLong()} that would be required to go from producing out1 to producing
         *         out2; can be positive or negative, and can be passed to {@link #skip(ulong)}
         */
        public static ulong Distance(ulong out1, ulong out2)
        {
            return (InverseNextULong(out2) - InverseNextULong(out1)) * 0xF1DE83E19937733DUL;
        }
        public uint NextUInt(uint bound)
        {
            return (uint)((bound * ((ulong)NextUInt())) >> 32);
        }
        public ulong NextULong(ulong bound)
        {
            ulong rand = NextULong();
            ulong randLow = rand & 0xFFFFFFFFUL;
            ulong boundLow = bound & 0xFFFFFFFFUL;
            rand >>= 32;
            bound >>= 32;
            ulong z = (randLow * boundLow >> 32);
            ulong t = rand * boundLow + z;
            return rand * bound + (t >> 32) + ((t & 0xFFFFFFFFUL) + randLow * bound >> 32) + (z >> 63);
        }
        /**
         * Returns a value between min (inclusive) and max (exclusive) as ints.
         * <br>
         * The inclusive and exclusive behavior is to match the behavior of the similar
         * method that deals with floating point values.
         * <br>
         * If {@code min} and {@code max} happen to be the same, {@code min} is returned
         * (breaking the exclusive behavior, but it's convenient to do so).
         *
         * @param min the minimum bound on the return value (inclusive)
         * @param max the maximum bound on the return value (exclusive)
         * @return the found value
         */
        public uint Between(uint min, uint max)
        {
            return NextUInt(max - min) + min;
        }

        /**
         * Returns a value between min (inclusive) and max (exclusive) as longs.
         * <br>
         * The inclusive and exclusive behavior is to match the behavior of the similar
         * method that deals with floating point values.
         * <br>
         * If {@code min} and {@code max} happen to be the same, {@code min} is returned
         * (breaking the exclusive behavior, but it's convenient to do so).
         *
         * @param min the minimum bound on the return value (inclusive)
         * @param max the maximum bound on the return value (exclusive)
         * @return the found value
         */
        public ulong Between(ulong min, ulong max)
        {
            return NextULong(max - min) + min;
        }
        public double NextDouble(double outer)
        {
            return NextDouble() * outer;
        }

        /**
         * Returns a value from a uniform distribution from min (inclusive) to max
         * (exclusive).
         *
         * @param min the minimum bound on the return value (inclusive)
         * @param max the maximum bound on the return value (exclusive)
         * @return the found value
         */
        public double Between(double min, double max)
        {
            return NextDouble(max - min) + min;
        }

    }
}
