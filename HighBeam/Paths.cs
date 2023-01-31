using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighBeam
{
    public static class Paths
    {
        public static List<LaneModel> PathList = new List<LaneModel>() {
            new LaneModel()
{
Name = "lane",
PathList = new List<PathModel>(){new PathModel()
{
Position = new Vector3(3254, 2029, 45),
Direction = 330.736f,
Speed = 4,
Steer = -7.681899E-05f
},new PathModel()
{
Position = new Vector3(3254, 2029, 45),
Direction = 330.7359f,
Speed = 5,
Steer = -5.963958E-06f
},new PathModel()
{
Position = new Vector3(3255, 2030, 45),
Direction = 330.7362f,
Speed = 6,
Steer = -1.028402E-06f
},new PathModel()
{
Position = new Vector3(3255, 2031, 45),
Direction = 330.8925f,
Speed = 8,
Steer = 0.2944414f
},new PathModel()
{
Position = new Vector3(3256, 2033, 45),
Direction = 332.1874f,
Speed = 9,
Steer = 0.02678505f
},new PathModel()
{
Position = new Vector3(3257, 2034, 45),
Direction = 332.8728f,
Speed = 10,
Steer = 0.005233892f
},new PathModel()
{
Position = new Vector3(3258, 2036, 45),
Direction = 332.5929f,
Speed = 12,
Steer = -0.3065185f
},new PathModel()
{
Position = new Vector3(3259, 2038, 45),
Direction = 330.8734f,
Speed = 13,
Steer = -0.06367483f
},new PathModel()
{
Position = new Vector3(3260, 2040, 45),
Direction = 329.9932f,
Speed = 15,
Steer = -0.01414173f
},new PathModel()
{
Position = new Vector3(3261, 2043, 45),
Direction = 330.0421f,
Speed = 16,
Steer = 0.06802821f
},new PathModel()
{
Position = new Vector3(3263, 2045, 45),
Direction = 330.5627f,
Speed = 16,
Steer = 0.01620798f
},new PathModel()
{
Position = new Vector3(3264, 2048, 45),
Direction = 330.7679f,
Speed = 17,
Steer = 0.003984492f
},new PathModel()
{
Position = new Vector3(3266, 2050, 45),
Direction = 330.807f,
Speed = 18,
Steer = 0.005473036f
},new PathModel()
{
Position = new Vector3(3267, 2053, 45),
Direction = 330.8142f,
Speed = 19,
Steer = 0.001436927f
},new PathModel()
{
Position = new Vector3(3269, 2056, 45),
Direction = 330.8147f,
Speed = 20,
Steer = 0.0003807896f
},new PathModel()
{
Position = new Vector3(3271, 2059, 45),
Direction = 330.8139f,
Speed = 20,
Steer = 0.0001122875f
},new PathModel()
{
Position = new Vector3(3273, 2063, 45),
Direction = 330.8128f,
Speed = 20,
Steer = 2.945827E-05f
},new PathModel()
{
Position = new Vector3(3274, 2066, 45),
Direction = 330.8119f,
Speed = 20,
Steer = 8.303783E-06f
},new PathModel()
{
Position = new Vector3(3276, 2069, 45),
Direction = 330.8116f,
Speed = 20,
Steer = 2.316835E-06f
},new PathModel()
{
Position = new Vector3(3278, 2072, 45),
Direction = 330.8115f,
Speed = 21,
Steer = 6.7453E-07f
},new PathModel()
{
Position = new Vector3(3280, 2076, 45),
Direction = 330.8515f,
Speed = 22,
Steer = 0.1548538f
},new PathModel()
{
Position = new Vector3(3282, 2079, 45),
Direction = 332.8873f,
Speed = 23,
Steer = 0.2933308f
},new PathModel()
{
Position = new Vector3(3284, 2083, 45),
Direction = 335.1652f,
Speed = 24,
Steer = 0.09753692f
},new PathModel()
{
Position = new Vector3(3285, 2087, 45),
Direction = 335.4969f,
Speed = 24,
Steer = -0.3789099f
},new PathModel()
{
Position = new Vector3(3287, 2091, 45),
Direction = 331.8103f,
Speed = 25,
Steer = -0.415347f
},new PathModel()
{
Position = new Vector3(3290, 2095, 45),
Direction = 328.438f,
Speed = 26,
Steer = -0.1371906f
},new PathModel()
{
Position = new Vector3(3292, 2099, 45),
Direction = 327.7075f,
Speed = 27,
Steer = -0.04671531f
},new PathModel()
{
Position = new Vector3(3295, 2103, 45),
Direction = 327.8229f,
Speed = 27,
Steer = 0.07127627f
},new PathModel()
{
Position = new Vector3(3297, 2107, 45),
Direction = 328.9919f,
Speed = 28,
Steer = 0.3696122f
},new PathModel()
{
Position = new Vector3(3300, 2111, 45),
Direction = 331.4586f,
Speed = 29,
Steer = 0.1543384f
},new PathModel()
{
Position = new Vector3(3302, 2116, 45),
Direction = 332.4712f,
Speed = 29,
Steer = 0.06119283f
},new PathModel()
{
Position = new Vector3(3304, 2120, 45),
Direction = 332.4758f,
Speed = 30,
Steer = -0.1204467f
},new PathModel()
{
Position = new Vector3(3307, 2125, 45),
Direction = 331.353f,
Speed = 31,
Steer = -0.1044427f
},new PathModel()
{
Position = new Vector3(3309, 2130, 45),
Direction = 330.5821f,
Speed = 31,
Steer = -0.0506988f
},new PathModel()
{
Position = new Vector3(3312, 2135, 45),
Direction = 330.2545f,
Speed = 32,
Steer = -0.05017802f
},new PathModel()
{
Position = new Vector3(3315, 2140, 45),
Direction = 330.0565f,
Speed = 32,
Steer = -0.01935166f
},new PathModel()
{
Position = new Vector3(3318, 2144, 45),
Direction = 330.0165f,
Speed = 33,
Steer = -0.00797155f
},new PathModel()
{
Position = new Vector3(3321, 2149, 45),
Direction = 330.1608f,
Speed = 33,
Steer = 0.06832622f
},new PathModel()
{
Position = new Vector3(3324, 2154, 45),
Direction = 330.561f,
Speed = 33,
Steer = 0.0268348f
},new PathModel()
{
Position = new Vector3(3327, 2160, 45),
Direction = 330.7156f,
Speed = 34,
Steer = 0.01024518f
},new PathModel()
{
Position = new Vector3(3330, 2165, 45),
Direction = 330.928f,
Speed = 34,
Steer = 0.1398891f
},new PathModel()
{
Position = new Vector3(3332, 2170, 45),
Direction = 331.665f,
Speed = 35,
Steer = 0.05907529f
},new PathModel()
{
Position = new Vector3(3335, 2176, 45),
Direction = 332.0203f,
Speed = 35,
Steer = 0.02282824f
},new PathModel()
{
Position = new Vector3(3338, 2181, 45),
Direction = 332.0402f,
Speed = 36,
Steer = -0.02886195f
},new PathModel()
{
Position = new Vector3(3341, 2187, 45),
Direction = 331.6f,
Speed = 36,
Steer = -0.07666072f
},new PathModel()
{
Position = new Vector3(3344, 2192, 45),
Direction = 331.0996f,
Speed = 37,
Steer = -0.02973616f
},new PathModel()
{
Position = new Vector3(3348, 2198, 45),
Direction = 330.9872f,
Speed = 37,
Steer = -0.01169919f
},new PathModel()
{
Position = new Vector3(3351, 2204, 45),
Direction = 330.9735f,
Speed = 38,
Steer = -0.004520116f
},new PathModel()
{
Position = new Vector3(3354, 2210, 45),
Direction = 330.9726f,
Speed = 38,
Steer = -0.001775053f
},new PathModel()
{
Position = new Vector3(3357, 2216, 45),
Direction = 330.9736f,
Speed = 39,
Steer = -0.0007340697f
},new PathModel()
{
Position = new Vector3(3361, 2222, 45),
Direction = 330.9748f,
Speed = 39,
Steer = -0.0002817124f
},new PathModel()
{
Position = new Vector3(3364, 2228, 45),
Direction = 330.9759f,
Speed = 40,
Steer = -0.0001097845f
},new PathModel()
{
Position = new Vector3(3368, 2235, 45),
Direction = 331.04f,
Speed = 40,
Steer = 0.05160883f
},new PathModel()
{
Position = new Vector3(3371, 2241, 45),
Direction = 331.337f,
Speed = 41,
Steer = 0.02358321f
},new PathModel()
{
Position = new Vector3(3375, 2247, 45),
Direction = 331.4442f,
Speed = 41,
Steer = 0.009828191f
},new PathModel()
{
Position = new Vector3(3378, 2254, 45),
Direction = 331.4638f,
Speed = 41,
Steer = 0.003805679f
},new PathModel()
{
Position = new Vector3(3382, 2261, 45),
Direction = 331.467f,
Speed = 42,
Steer = 0.00138126f
},new PathModel()
{
Position = new Vector3(3386, 2268, 45),
Direction = 331.4654f,
Speed = 42,
Steer = -0.01474695f
},new PathModel()
{
Position = new Vector3(3389, 2274, 45),
Direction = 331.2213f,
Speed = 43,
Steer = -0.03858263f
},new PathModel()
{
Position = new Vector3(3393, 2281, 45),
Direction = 331.004f,
Speed = 43,
Steer = -0.01455286f
},new PathModel()
{
Position = new Vector3(3397, 2288, 45),
Direction = 330.9636f,
Speed = 43,
Steer = -0.005595201f
},new PathModel()
{
Position = new Vector3(3401, 2295, 45),
Direction = 330.9574f,
Speed = 44,
Steer = -0.002169085f
},new PathModel()
{
Position = new Vector3(3405, 2302, 45),
Direction = 330.9571f,
Speed = 44,
Steer = -0.0009124181f
},new PathModel()
{
Position = new Vector3(3408, 2309, 45),
Direction = 330.9577f,
Speed = 44,
Steer = -0.000359136f
},new PathModel()
{
Position = new Vector3(3412, 2315, 45),
Direction = 330.9583f,
Speed = 45,
Steer = -0.0001491331f
},new PathModel()
{
Position = new Vector3(3416, 2322, 45),
Direction = 330.9587f,
Speed = 45,
Steer = -5.826614E-05f
},new PathModel()
{
Position = new Vector3(3420, 2329, 45),
Direction = 330.9588f,
Speed = 45,
Steer = -2.377407E-05f
},new PathModel()
{
Position = new Vector3(3424, 2336, 45),
Direction = 330.9585f,
Speed = 45,
Steer = -0.009214142f
},new PathModel()
{
Position = new Vector3(3428, 2343, 45),
Direction = 330.9044f,
Speed = 46,
Steer = -0.01040835f
},new PathModel()
{
Position = new Vector3(3432, 2350, 45),
Direction = 330.8833f,
Speed = 46,
Steer = -0.004318894f
},new PathModel()
{
Position = new Vector3(3436, 2358, 45),
Direction = 330.8772f,
Speed = 46,
Steer = -0.001656614f
},new PathModel()
{
Position = new Vector3(3440, 2365, 45),
Direction = 330.8758f,
Speed = 47,
Steer = -0.0006382525f
},new PathModel()
{
Position = new Vector3(3444, 2373, 45),
Direction = 330.8756f,
Speed = 47,
Steer = -0.0002452f
},new PathModel()
{
Position = new Vector3(3448, 2380, 45),
Direction = 330.8755f,
Speed = 47,
Steer = -0.0001000622f
},new PathModel()
{
Position = new Vector3(3452, 2387, 45),
Direction = 330.8753f,
Speed = 48,
Steer = -4.184268E-05f
},new PathModel()
{
Position = new Vector3(3456, 2394, 45),
Direction = 330.8753f,
Speed = 48,
Steer = -1.73786E-05f
},new PathModel()
{
Position = new Vector3(3460, 2401, 45),
Direction = 330.8752f,
Speed = 48,
Steer = -6.829188E-06f
},new PathModel()
{
Position = new Vector3(3464, 2408, 45),
Direction = 330.8751f,
Speed = 49,
Steer = -2.795523E-06f
},new PathModel()
{
Position = new Vector3(3468, 2416, 45),
Direction = 330.8748f,
Speed = 49,
Steer = -1.069979E-06f
},new PathModel()
{
Position = new Vector3(3473, 2424, 45),
Direction = 330.8745f,
Speed = 50,
Steer = -4.228451E-07f
},new PathModel()
{
Position = new Vector3(3477, 2432, 45),
Direction = 330.8742f,
Speed = 50,
Steer = -1.619738E-07f
},new PathModel()
{
Position = new Vector3(3482, 2440, 45),
Direction = 330.8741f,
Speed = 50,
Steer = -6.254112E-08f
},new PathModel()
{
Position = new Vector3(3486, 2448, 45),
Direction = 330.8741f,
Speed = 51,
Steer = -2.400021E-08f
},new PathModel()
{
Position = new Vector3(3491, 2456, 45),
Direction = 330.874f,
Speed = 51,
Steer = -9.021618E-09f
},new PathModel()
{
Position = new Vector3(3495, 2464, 45),
Direction = 330.8737f,
Speed = 51,
Steer = -3.540206E-09f
},new PathModel()
{
Position = new Vector3(3499, 2472, 45),
Direction = 330.8734f,
Speed = 51,
Steer = -1.471064E-09f
},new PathModel()
{
Position = new Vector3(3504, 2480, 45),
Direction = 330.8733f,
Speed = 52,
Steer = -5.700314E-10f
},new PathModel()
{
Position = new Vector3(3508, 2488, 45),
Direction = 330.8731f,
Speed = 52,
Steer = 0.0005880556f
},new PathModel()
{
Position = new Vector3(3512, 2495, 45),
Direction = 330.8729f,
Speed = 52,
Steer = 0.0002449995f
},new PathModel()
{
Position = new Vector3(3517, 2504, 45),
Direction = 330.8727f,
Speed = 53,
Steer = 9.562777E-05f
},new PathModel()
{
Position = new Vector3(3522, 2512, 45),
Direction = 330.8725f,
Speed = 53,
Steer = 3.716084E-05f
},new PathModel()
{
Position = new Vector3(3526, 2520, 45),
Direction = 330.8723f,
Speed = 53,
Steer = 1.410544E-05f
},new PathModel()
{
Position = new Vector3(3531, 2529, 45),
Direction = 330.8723f,
Speed = 53,
Steer = 5.418027E-06f
},new PathModel()
{
Position = new Vector3(3536, 2537, 45),
Direction = 330.8722f,
Speed = 54,
Steer = 2.272025E-06f
},new PathModel()
{
Position = new Vector3(3540, 2545, 45),
Direction = 330.8721f,
Speed = 54,
Steer = 9.20155E-07f
},new PathModel()
{
Position = new Vector3(3544, 2553, 45),
Direction = 330.872f,
Speed = 54,
Steer = 3.676456E-07f
},new PathModel()
{
Position = new Vector3(3549, 2562, 45),
Direction = 330.8719f,
Speed = 54,
Steer = 1.428617E-07f
},new PathModel()
{
Position = new Vector3(3554, 2570, 45),
Direction = 330.989f,
Speed = 55,
Steer = 0.03877323f
},new PathModel()
{
Position = new Vector3(3559, 2579, 45),
Direction = 331.1601f,
Speed = 55,
Steer = 0.01537723f
},new PathModel()
{
Position = new Vector3(3564, 2588, 45),
Direction = 331.1948f,
Speed = 54,
Steer = 0.00593182f
},new PathModel()
{
Position = new Vector3(3568, 2596, 45),
Direction = 331.1935f,
Speed = 53,
Steer = 0.002292251f
},new PathModel()
{
Position = new Vector3(3573, 2604, 45),
Direction = 331.1878f,
Speed = 52,
Steer = 0.0009438079f
},new PathModel()
{
Position = new Vector3(3577, 2612, 45),
Direction = 331.1864f,
Speed = 51,
Steer = 0.02448978f
},new PathModel()
{
Position = new Vector3(3582, 2620, 45),
Direction = 331.4241f,
Speed = 50,
Steer = 0.03272975f
},new PathModel()
{
Position = new Vector3(3586, 2628, 45),
Direction = 331.5755f,
Speed = 49,
Steer = 0.01257187f
},new PathModel()
{
Position = new Vector3(3590, 2636, 45),
Direction = 331.6007f,
Speed = 48,
Steer = -0.001737587f
},new PathModel()
{
Position = new Vector3(3594, 2644, 45),
Direction = 331.6001f,
Speed = 47,
Steer = -0.0006743402f
},new PathModel()
{
Position = new Vector3(3598, 2651, 45),
Direction = 331.5944f,
Speed = 46,
Steer = -0.0002801035f
},new PathModel()
{
Position = new Vector3(3602, 2658, 45),
Direction = 331.5863f,
Speed = 45,
Steer = -0.0001070461f
},new PathModel()
{
Position = new Vector3(3606, 2665, 45),
Direction = 331.578f,
Speed = 44,
Steer = -4.161829E-05f
},new PathModel()
{
Position = new Vector3(3610, 2672, 45),
Direction = 331.572f,
Speed = 44,
Steer = -1.727035E-05f
},new PathModel()
{
Position = new Vector3(3613, 2679, 45),
Direction = 331.57f,
Speed = 44,
Steer = -6.682721E-06f
},new PathModel()
{
Position = new Vector3(3617, 2686, 45),
Direction = 331.4877f,
Speed = 43,
Steer = -0.0472605f
},new PathModel()
{
Position = new Vector3(3621, 2693, 45),
Direction = 331.2366f,
Speed = 43,
Steer = -0.01802478f
},new PathModel()
{
Position = new Vector3(3624, 2699, 45),
Direction = 331.1688f,
Speed = 43,
Steer = -0.007475682f
},new PathModel()
{
Position = new Vector3(3628, 2706, 45),
Direction = 331.1549f,
Speed = 43,
Steer = -0.002874202f
},new PathModel()
{
Position = new Vector3(3632, 2713, 45),
Direction = 331.1502f,
Speed = 42,
Steer = -0.001106591f
},new PathModel()
{
Position = new Vector3(3636, 2719, 45),
Direction = 331.141f,
Speed = 40,
Steer = -0.000425202f
},new PathModel()
{
Position = new Vector3(3639, 2726, 45),
Direction = 331.1298f,
Speed = 39,
Steer = -0.0001649964f
},new PathModel()
{
Position = new Vector3(3642, 2732, 45),
Direction = 331.1161f,
Speed = 37,
Steer = -6.310711E-05f
},new PathModel()
{
Position = new Vector3(3645, 2737, 45),
Direction = 331.1028f,
Speed = 36,
Steer = -2.619231E-05f
},new PathModel()
{
Position = new Vector3(3648, 2742, 45),
Direction = 331.0884f,
Speed = 34,
Steer = -1.062955E-05f
},new PathModel()
{
Position = new Vector3(3651, 2748, 45),
Direction = 331.0728f,
Speed = 33,
Steer = -4.140541E-06f
},new PathModel()
{
Position = new Vector3(3654, 2753, 45),
Direction = 331.0609f,
Speed = 32,
Steer = -1.611372E-06f
},new PathModel()
{
Position = new Vector3(3657, 2758, 45),
Direction = 331.057f,
Speed = 32,
Steer = -6.20227E-07f
},new PathModel()
{
Position = new Vector3(3660, 2763, 45),
Direction = 331.0552f,
Speed = 32,
Steer = -2.363058E-07f
},new PathModel()
{
Position = new Vector3(3663, 2768, 45),
Direction = 331.0537f,
Speed = 32,
Steer = -9.12929E-08f
},new PathModel()
{
Position = new Vector3(3665, 2773, 45),
Direction = 331.0523f,
Speed = 31,
Steer = -3.511536E-08f
},new PathModel()
{
Position = new Vector3(3668, 2778, 45),
Direction = 331.0511f,
Speed = 31,
Steer = -1.451294E-08f
},new PathModel()
{
Position = new Vector3(3671, 2783, 45),
Direction = 331.0498f,
Speed = 31,
Steer = -5.514591E-09f
},new PathModel()
{
Position = new Vector3(3673, 2787, 45),
Direction = 331.0471f,
Speed = 30,
Steer = -2.343038E-09f
},new PathModel()
{
Position = new Vector3(3676, 2792, 45),
Direction = 331.0384f,
Speed = 29,
Steer = -8.821864E-10f
},new PathModel()
{
Position = new Vector3(3678, 2797, 45),
Direction = 331.0347f,
Speed = 29,
Steer = -3.291387E-10f
},new PathModel()
{
Position = new Vector3(3681, 2801, 45),
Direction = 331.0332f,
Speed = 29,
Steer = -1.30933E-10f
},new PathModel()
{
Position = new Vector3(3683, 2805, 45),
Direction = 331.032f,
Speed = 28,
Steer = -5.188273E-11f
},new PathModel()
{
Position = new Vector3(3685, 2810, 45),
Direction = 331.0308f,
Speed = 28,
Steer = -1.953531E-11f
},new PathModel()
{
Position = new Vector3(3688, 2814, 45),
Direction = 330.9848f,
Speed = 28,
Steer = -0.04964392f
},new PathModel()
{
Position = new Vector3(3690, 2818, 45),
Direction = 330.7292f,
Speed = 28,
Steer = -0.01835274f
},new PathModel()
{
Position = new Vector3(3693, 2822, 45),
Direction = 330.6118f,
Speed = 28,
Steer = -0.007054549f
},new PathModel()
{
Position = new Vector3(3695, 2827, 45),
Direction = 330.5855f,
Speed = 27,
Steer = -0.002538537f
},new PathModel()
{
Position = new Vector3(3697, 2831, 45),
Direction = 330.5778f,
Speed = 27,
Steer = -0.0008969407f
},new PathModel()
{
Position = new Vector3(3700, 2835, 45),
Direction = 330.5757f,
Speed = 27,
Steer = -0.0003467841f
},new PathModel()
{
Position = new Vector3(3702, 2839, 45),
Direction = 330.5624f,
Speed = 28,
Steer = -0.02623448f
},new PathModel()
{
Position = new Vector3(3704, 2843, 45),
Direction = 330.3122f,
Speed = 28,
Steer = -0.0667512f
},new PathModel()
{
Position = new Vector3(3707, 2847, 45),
Direction = 329.8915f,
Speed = 28,
Steer = -0.1056312f
},new PathModel()
{
Position = new Vector3(3709, 2852, 45),
Direction = 328.5817f,
Speed = 28,
Steer = -0.2069259f
},new PathModel()
{
Position = new Vector3(3712, 2856, 45),
Direction = 326.9657f,
Speed = 28,
Steer = -0.1721115f
},new PathModel()
{
Position = new Vector3(3715, 2860, 45),
Direction = 325.8578f,
Speed = 28,
Steer = -0.1435494f
},new PathModel()
{
Position = new Vector3(3718, 2865, 45),
Direction = 324.7523f,
Speed = 28,
Steer = -0.2636486f
},new PathModel()
{
Position = new Vector3(3721, 2869, 45),
Direction = 323.0724f,
Speed = 29,
Steer = -0.1598618f
},new PathModel()
{
Position = new Vector3(3723, 2873, 45),
Direction = 321.8859f,
Speed = 29,
Steer = -0.1941254f
},new PathModel()
{
Position = new Vector3(3726, 2876, 45),
Direction = 320.783f,
Speed = 29,
Steer = -0.1136756f
},new PathModel()
{
Position = new Vector3(3730, 2880, 45),
Direction = 319.819f,
Speed = 28,
Steer = -0.253512f
},new PathModel()
{
Position = new Vector3(3733, 2884, 45),
Direction = 318.2548f,
Speed = 28,
Steer = -0.2147495f
},new PathModel()
{
Position = new Vector3(3737, 2888, 45),
Direction = 316.3427f,
Speed = 28,
Steer = -0.2135866f
},new PathModel()
{
Position = new Vector3(3740, 2892, 45),
Direction = 314.5052f,
Speed = 28,
Steer = -0.2605932f
},new PathModel()
{
Position = new Vector3(3744, 2895, 45),
Direction = 312.6813f,
Speed = 29,
Steer = -0.3465442f
},new PathModel()
{
Position = new Vector3(3747, 2899, 45),
Direction = 310.2723f,
Speed = 29,
Steer = -0.2546468f
},new PathModel()
{
Position = new Vector3(3751, 2902, 45),
Direction = 308.2065f,
Speed = 29,
Steer = -0.272872f
},new PathModel()
{
Position = new Vector3(3756, 2905, 45),
Direction = 306.5091f,
Speed = 29,
Steer = -0.2046926f
},new PathModel()
{
Position = new Vector3(3760, 2909, 45),
Direction = 305.0643f,
Speed = 29,
Steer = -0.1670755f
},new PathModel()
{
Position = new Vector3(3764, 2912, 45),
Direction = 303.8193f,
Speed = 29,
Steer = -0.2856798f
},new PathModel()
{
Position = new Vector3(3768, 2914, 45),
Direction = 301.9587f,
Speed = 29,
Steer = -0.2485836f
},new PathModel()
{
Position = new Vector3(3773, 2917, 45),
Direction = 300.0723f,
Speed = 29,
Steer = -0.2839908f
},new PathModel()
{
Position = new Vector3(3777, 2919, 45),
Direction = 298.1101f,
Speed = 29,
Steer = -0.3101422f
},new PathModel()
{
Position = new Vector3(3781, 2922, 45),
Direction = 295.9217f,
Speed = 29,
Steer = -0.320771f
},new PathModel()
{
Position = new Vector3(3786, 2924, 45),
Direction = 293.6232f,
Speed = 29,
Steer = -0.3253087f
},new PathModel()
{
Position = new Vector3(3791, 2926, 45),
Direction = 291.3584f,
Speed = 29,
Steer = -0.2741252f
},new PathModel()
{
Position = new Vector3(3796, 2928, 45),
Direction = 289.5544f,
Speed = 27,
Steer = -0.1560375f
},new PathModel()
{
Position = new Vector3(3800, 2929, 45),
Direction = 288.8665f,
Speed = 25,
Steer = -0.05785884f
},new PathModel()
{
Position = new Vector3(3804, 2931, 45),
Direction = 288.8117f,
Speed = 22,
Steer = -0.01805579f
},new PathModel()
{
Position = new Vector3(3807, 2932, 45),
Direction = 289.2807f,
Speed = 19,
Steer = 0.4829384f
},new PathModel()
{
Position = new Vector3(3810, 2933, 45),
Direction = 291.9855f,
Speed = 16,
Steer = 0.8645104f
},new PathModel()
{
Position = new Vector3(3812, 2934, 45),
Direction = 296.642f,
Speed = 14,
Steer = 0.9672341f
},new PathModel()
{
Position = new Vector3(3814, 2935, 45),
Direction = 302.2493f,
Speed = 11,
Steer = 0.9930586f
},new PathModel()
{
Position = new Vector3(3816, 2936, 45),
Direction = 307.5535f,
Speed = 9,
Steer = 0.998504f
},new PathModel()
{
Position = new Vector3(3817, 2937, 45),
Direction = 312.7266f,
Speed = 6,
Steer = 0.9996895f
},new PathModel()
{
Position = new Vector3(3817, 2938, 45),
Direction = 317.7496f,
Speed = 4,
Steer = 0.999948f
},}
}

        };
    }
}
