# Phantom Limb Pain - VR 
This repo is intended for virtual reality research at Rhodes College. See the menu below for more information.

- [Build Settings](#build-settings)
- [Pull Requests](#pull-requests)
- [Compiling to Oculus](#oculus)
- [Throwing Mechanics](#throwing mechanics)
- [UI](#ui-and-game-mechanics)




### Build Settings
- launch in Unity version 2020.3.9f1
- Run on Android platform
- Texture Compression: ASTC

### Pull Requests
Before you can request the project code on your local device, you must first fork the project to your Github account. You can do this by clicking the fork button on the top right of the repository. 

Next, you will clone the project. Click on the green Clone button, and copy the link. Open your computer's terminal and type 
```
  git clone https://github.com/[USERNAME]/Phantom_Limb_Pain.git. 
```
You should now have the project on your local device. 

### Oculus
Plug in the Oculus device using the USBC cable, plugging one end into the Oculus and one end into the computer. Once dev settings are enabled and the Project is on your local device, go to file > Build and Run. You can now find the App in the Oculus under APPS > Unknown Sources > Com.RhodesCollege.PhantomLimb. You can change the name under Edit > Player Settings > Player. 

### Throwing Mechanics
The throwing mechanics differ from the XR throwing mechanics in that 1. the velocity is taken from the center of mass of the controller and 2. the velocity applied to the object is the peak velocity in the last 5 frames of releasing the objects.
1. this is important so that the throwing is more realistic relative to the weight that the user is holding in hand.
2. this is important because release of the controller does not give a concrete feeling of releasing the ball. Grabbing the peak velocity is likely the point where the user would release the ball in real life and is likely what they would expect when throwing in VR. 
The same calculations for velocity are calculated and applied to angular velocity. 

### UI and Game Mechanics
The game manager reads a set of instructions to the user once they are ready. The instructions canvas disapears either when the instructions are finsihed or when the use picks up the ball (maybe change this). If the ball goes too far away, the user can respawn a ball at any time by pressing the button on the canvas behind them. 

The users task is to throw the ball at the upcoming targets. The target will spawn at a new randomized position each time one is hit. There are 12 total targets, an equal amount that spawn at 1, 2, 5, and 8 meters. Once all 12 targets are hit, the game will automaticall terminate.

Coded by London Bielicke. For more information contact me at bielm-24@rhodes.edu.
