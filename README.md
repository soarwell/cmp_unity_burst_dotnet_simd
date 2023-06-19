# unity

unity version 2022.3.0f1

[unity Sample in EntityComponentSystemSamples ](https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/EntitiesSamples/Assets/Tutorials/Jobs/README.md)

my setting is
NumSeekers = 2000
NumTargets = 2000
run in release mode
![image](https://github.com/soarwell/cmp_unity_burst_dotnet_simd/assets/19923734/f8d702f1-059d-4339-8400-cb77e971a417)

![image](https://github.com/soarwell/cmp_unity_burst_dotnet_simd/assets/19923734/eaa571a8-3409-4863-ae63-f495c9540fcb)

# dotnet 6

seeker and target number is same with unity, using System.Numerics.Vector3 in dotnet 6;
static public int NumSeekers = 2000;
static public int NumTargets = 2000;

in debug mode(press f5 in visual studio)
![image](https://github.com/soarwell/cmp_unity_burst_dotnet_simd/assets/19923734/a797344f-4f13-464a-bc0a-55b0d9718e59)

in release mode
![image](https://github.com/soarwell/cmp_unity_burst_dotnet_simd/assets/19923734/eb25afba-0fdd-4de1-8d54-10ea18818c9c)

from the test results, it seems that the Unity Burst compilation did not perform better than the Dotnet6 original program with SIMD enabled, At least in this example, they are similar










