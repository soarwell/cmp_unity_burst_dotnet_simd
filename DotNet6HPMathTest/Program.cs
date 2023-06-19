using System;
using System.Numerics;
using System.Diagnostics;

namespace ConsoleAppHPMathTest
{
    public struct Transform
    {
        public Vector3 localPosition;
        public Vector3 Direction;
    }
    public class Spawner// : MonoBehaviour
    {
        //public System.Numerics.Vector2
        public static Transform[] TargetTransforms;

        // Cache the seeker transforms.
        public static Transform[] SeekerTransforms;

       // public GameObject SeekerPrefab;
       // public GameObject TargetPrefab;
        static public int NumSeekers = 2000;
        static public int NumTargets = 2000;
        public Vector2 Bounds = new Vector2(500, 500);

        public void Start()
        {
            Random rand = new Random(123);

            SeekerTransforms = new Transform[NumSeekers];
            for (int i = 0; i < NumSeekers; i++)
            {
                //Seeker seeker = go.GetComponent<Seeker>();
                Vector2 dir = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble());
                SeekerTransforms[i].Direction = new Vector3(dir.X, 0, dir.Y);
                SeekerTransforms[i].localPosition = new Vector3(
                    rand.Next(0, (int)Bounds.X), 0, rand.Next(0, (int)Bounds.Y));
            }

            TargetTransforms = new Transform[NumTargets];
            for (int i = 0; i < NumTargets; i++)
            {
                Vector2 dir = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble());
                TargetTransforms[i].Direction = new Vector3(dir.X, 0, dir.Y);
                TargetTransforms[i].localPosition = new Vector3(
                    rand.Next(0, (int)Bounds.X), 0, rand.Next(0, (int)Bounds.Y));
            }
        }

        public void UpdateMove()
        {
            for (int i = 0; i < TargetTransforms.Length; i++)
            {
                // Vector3 is implicitly converted to float3
                TargetTransforms[i].localPosition += TargetTransforms[i].Direction * 0.01f;
            }

            // Copy every seeker transform to a NativeArray.
            for (int i = 0; i < SeekerTransforms.Length; i++)
            {
                // Vector3 is implicitly converted to float3
                SeekerTransforms[i].localPosition += SeekerTransforms[i].Direction * 0.01f;
            }
        }
    }

    

    public class FindNearest// : MonoBehaviour
    {
        // The size of our arrays does not need to vary, so rather than create
        // new arrays every field, we'll create the arrays in Awake() and store them
        // in these fields.
        Vector3[] TargetPositions;
        Vector3[] SeekerPositions;
        public Vector3[] NearestTargetPositions;

        public Vector3 total;

        public void Start()
        {
            // We use the Persistent allocator because these arrays must
            // exist for the run of the program.
            TargetPositions = new Vector3[Spawner.NumTargets];
            SeekerPositions = new Vector3[Spawner.NumSeekers];
            NearestTargetPositions = new Vector3[Spawner.NumSeekers];
        }

        public void Update()
        {
            // Copy every target transform to a NativeArray.
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                // Vector3 is implicitly converted to float3
                TargetPositions[i] = Spawner.TargetTransforms[i].localPosition;
            }

            // Copy every seeker transform to a NativeArray.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // Vector3 is implicitly converted to float3
                SeekerPositions[i] = Spawner.SeekerTransforms[i].localPosition;
            }

            // To schedule a job, we first need to create an instance and populate its fields.
            Execute();

            // Draw a debug line from each seeker to its nearest target.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // float3 is implicitly converted to Vector3
                total += (SeekerPositions[i] + NearestTargetPositions[i]);
            }
        }

        public void Execute()
        {
            // Compute the square distance from each seeker to every target.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                Vector3 seekerPos = SeekerPositions[i];
                float nearestDistSq = float.MaxValue;
                for (int j = 0; j < TargetPositions.Length; j++)
                {
                    Vector3 targetPos = TargetPositions[j];
                    float distSq = Vector3.DistanceSquared(seekerPos, targetPos);
                    if (distSq < nearestDistSq)
                    {
                        nearestDistSq = distSq;
                        NearestTargetPositions[i] = targetPos;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool simdenable = Vector.IsHardwareAccelerated;

            Spawner spawner = new Spawner();
            spawner.Start();

            FindNearest fn = new FindNearest();

            fn.Start();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            long emsPrint = 1000;
            int fps = 0;
            long emsFrameStart = 0;
            while (sw.ElapsedMilliseconds < 10000)
            {
                spawner.UpdateMove();
                fn.Update();
                fps++;
                var tmp = sw.ElapsedMilliseconds;

                if (tmp >= emsPrint)
                {
                    if(fps == 0)
                    {
                        fps = 1;
                    }

                    
                    Console.WriteLine("fps {0} time {1} total {2}", fps, (tmp - emsFrameStart) / fps, fn.total);

                    emsPrint += 1000;
                    fps = 0;

                    emsFrameStart = tmp;

                    fn.total = Vector3.Zero;
                }
            }

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }
}
