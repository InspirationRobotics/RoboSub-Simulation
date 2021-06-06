# TransdecEnvironment

![image](https://user-images.githubusercontent.com/23311513/53693770-1e5fbe00-3da5-11e9-8d21-36170c0b334d.png)

TransdecEnvironment is a Unity3D project prepared for RoboSub competition by PWr Diving Crew (KN Robocik) at Wrocław University of Science and Technology.

The simulation environment was originally prepared for Robosub 2018 Competition. It is built on TRANSDEC facility model from [SimBox repository](https://github.com/cantren/cantren.github.io).

The project is maintained by PWr Diving Crew software team members (Unity3D section).

[KN Robocik website](http://www.robocik.pwr.edu.pl/)

Should any issues be noticed, please submit a **New issue** on Github.

## Installation

### Unity
**TransdecEnvironment** requires latest stable version of Unity development platform and is platform-independent. For Windows and Mac OS X versions visit their site: https://unity3d.com/get-unity/download.

### Python
For instructions on Python API configuration please go to [**PyTransdec** repository](https://github.com/PiotrJZielinski/PyTransdec).

### The environment
Once installed, clone TransdecEnvironment GitHub repository by executing this command:

```git clone https://github.com/PiotrJZielinski/TransdecEnvironment```

To use the environment launch the Unity Editor and open the project.

![image](https://user-images.githubusercontent.com/23311513/53694275-cd53c800-3dac-11e9-95e2-1f3830b64635.png)

In the project, make sure that `Scripting Runtime Version` is set to **`.Net 4.x Equivalent`**:
![image](https://user-images.githubusercontent.com/23311513/53694528-6afcc680-3db0-11e9-8d1e-250390f7988f.png)

Having done all of the above steps, double-click `RoboSub2018` scene in `Assets/Scenes` directory to open the default TransdecEnvironment scene.

![image](https://user-images.githubusercontent.com/23311513/53694507-296c1b80-3db0-11e9-87df-822953e76585.png)

### Optional Paid Assets

![image](https://i.imgur.com/5xr5I3y.gif)

*Aura 2* is a paid asset used in the project for more realistic lightning. We can't post it in this repository as it would be against the license.

If you're not a member of PWr Diving Crew and you want to use it you need to buy *Aura 2* on the [Unity Asset Store](https://assetstore.unity.com/).

After you have an account with *Aura 2* bought, go back to Unity and select `Window/Asset Store` in the menu. Then search for *Aura 2* package and import it into the project.

If you don't have it you can still run the simulation, although there will be some warnings about missing scripts in the console. 

You'll also have to comment out `#define HAVE_AURA_2` in `Assets/Standard Assets/Environment/Water/Water4/Shaders/FXWater4Advanced.shader`.


## Usage

**TransdecEnvironment** configuration is held by **Academy** object:

![image](https://user-images.githubusercontent.com/23311513/53694612-9207c800-3db1-11e9-99b7-70c264d01e26.png)

Click it to show its properties in the Inspector:

![image](https://user-images.githubusercontent.com/23311513/53694634-ddba7180-3db1-11e9-97d6-8bfb218bc361.png)

### Essential Academy parameters:
  * **Training Configuration** - visual observations' settings used when *training mode* is selected in Python API
  * **Inference Configuration** - visual observations' settings used when *training mode* is **not** selected in Python API
    * `Width` - camera image width
    * `Height` - camera image height (both affect performance)
    * `Quality Level` - image compression setting (`1` - lowest quality, `5` - highest quality)
    * `Time Scale` - indicates how many frames are dropped during communication (ie. how "fast" the environment behave)
    * `Target Frame Rate` - how many frames per secodn should the environment return (`-1` for maximum available)
    
  * **Reset Parameters** - parameters that can be set using Python API on environment reset (better not to modify unless you know what you are doing)
    * `CollectData` - set environment in data collection mode (`0` - standard mode, `1` - data collection mode)
    * `EnableNoise` - only used when `CollectData == 1`; enable random positioning of other objects (so that they create "noise")
    * `Positive` - only used when `CollectData == 1`; collect positive examples (`0` - negative examples, target invisible, `1` - positive examples, target visible)
    * `AgentMaxSteps` - how many steps the agent can take before resetting the environment (defaults to `0` - indefinite steps)
    
  * **Controller Settings**:
    * `Control` - agent steering method (`Player` for keyboard steering, `Python` for Python API controller); if `Control == Player` use keyboard for steering:
      * `W` - `S`: longitudinal movement (front-backward)
      * `A` - `D`: lateral movement (left-right)
      * `R` - `F`: vertical movement (upward-downward)
      * `Q` - `E`: yaw rotation (turn left-turn right)
    * `Learning Brain`, `Player Brain` - ML-Agents Brain objects (correctly set by default)
    
  * **Start position settings** - starting position drawing settings:
    * `Random Quarter` - randomly select one of 4 TRANSDEC quarters (if `false` default quarter is chosen)
    * `Random Position` - randomly move all objects on reset (if `false` objects stay in their default position)
    * `Random Orientation` - enable random rotation of the agent (at an angle of 90° or 180° to the gate)
    
  * **Data collection settings** - used when `resetParameters["CollectData"] == 1`
    * `Mode` - target whose images are collected
    * `Gate Target Object`, `Path Target Object`, ... - target objects from the environment
    
  * **Debug settings** (you shouldn't reallly touch them)
    * `Force Data Collection` - execute data collection regardless of controller mode
    * `Force Noise` - execute noised data collection
    * `Force Negative Examples` - execute negative examples data collection

### Debug Controls
 * `B` to pause the simulation in editor and display debug screen
 * `V` to toggle debug screen on and off
    
## Updating
In order to update TransdecEnvironment you need to reset your changes by executing:

```git stash```

You can then pull latest changes from `master` branch by executing:

```git pull```

In case you want to reapply changes you can execute

```git stash pop```

but this is not guaranteed to be working. If you do not need your changes execute

``` git stash drop```

Please **do not** push your changes to `master` branch. If you find your changes useful please create another branch and create a **Pull Request**
