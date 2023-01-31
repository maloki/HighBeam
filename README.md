# HighBeam

HighBeam is a codename for a set of vehicle enhancing features for GTA V, using RAGE engine.
Project was started in 2015, when GTA V scripting came into a picture, 
and it meant to be a simple car low/high beam switcher script, and it quickly became a sandbox for testing various game engine possibilities.

Written in C#.

All of the features are plugins, so that means they can be used independently. 

List of some features with short description:

- Traction controll system (TCS)
    Prevents car wheels from loosing grip
    
- Electronic stability program (ESP)
    Real world implementation of esp, which prevents car from skidding (feels like real BOSCH ESP).
    Contains normal and sport mode (sport allows you to slide more, but eventually esp will kick in).
    
- Matrix headlights (AHL)
    Each headlight contains multiple separate leds segments, 
    and when high beam headlight mode is active, each led light can be turned OFF or ON depending on conditions,
    to not dazzle other drivers on the road. 
    It can detect up to 10 vehicles, which is more than enough considering GTA V traffic.
    
- Automatic gearbox
    New implementation of real world automatic gearbox, with PRND modes.
    It also contains Creep, which represents car moving slowly forwards up to 8 km/h when in D gear.
    
- Addaptive Cruise Control
    In addition to regular cruise control, where you can set up speed, it also can follow a car ahead,
    adjusting speed and steering (works in corners as well). 

- Active brake force distribution
    GTA vanilla braking system is very simple, its either braking or not, depending on your controller trigger. 
    This feature is trying to recreate realistic BFD, you can find in real cars. 
    Each wheel is controlled individualy, and depending on condition on the road, it can give or take brake pressure,
    to ensure car is braking effeci
