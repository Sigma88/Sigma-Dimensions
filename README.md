# *Sigma Dimensions*

*The Universal Rescale Mod for KSP*


### Downloads
- [GitHub](https://github.com/Sigma88/Sigma-Dimensions/releases)
- [CurseForge](https://www.curseforge.com/kerbal/ksp-mods/sigma-dimensions) (Automatic install available)


### Other Links
- [ChangeLog](https://raw.githubusercontent.com/Sigma88/Sigma-Dimensions/master/Changelog.txt)
- [Source](https://github.com/Sigma88/Sigma-Dimensions/)


### Support
If you enjoy my mods, please consider supporting me with a small donation.

[![Donate Pounds](https://i.imgur.com/xBBQy19.png)][£][![Donate Euros](https://i.imgur.com/kKYb2lE.png)][€][![Donate Dollars](https://i.imgur.com/TT1Vymu.png)][$]

[£]: https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=65VBNHB39BTKG&item_name=Sigma-Dimensions&currency_code=GBP "Donate Pounds"
[€]: https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=65VBNHB39BTKG&item_name=Sigma-Dimensions&currency_code=EUR "Donate Euros"
[$]: https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=65VBNHB39BTKG&item_name=Sigma-Dimensions&currency_code=USD "Donate Dollars"


### How To Report A Bug

To report a bug or to ask for support, open an issue on [GitHub](https://github.com/Sigma88/Sigma-Dimensions/issues).


### How To Install

This mod can be installed and updated automatically using the [CurseForge App](https://curseforge.overwolf.com/).

To install the mod manually, follow these instructions:

**1. Install Manually**
- Remove any other version of the mod.
- Unzip the archive directly into the folder 'KSP\GameData'

**2. Uninstall manually**
- Delete the folder 'KSP\GameData\SigmaDimensions'


### License
All Rights Reserved



# Settings

```
SigmaDimensions
{
}
```

This is the settings node, it is provided by the mod and should be edited using
[ModuleManager](http://forum.kerbalspaceprogram.com/index.php?/topic/50533-0/) patches.

The SigmaDimensions settings node contains both Base and Advanced settings:

## Base Settings Definitions

  - **Resize**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Planetary Radius is multiplied by this value.

    The mass of each body is changed to maintain the same surface gravity.
    ```

  - **Rescale**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Orbit size (SemiMajor Axis) is multiplied by this value.
    ```

  - **Atmosphere**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Atmosphere height is multiplied by this value.
    ```

  - **dayLengthMultiplier**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Rotation period is multiplied by this value.

    Does not affect tidally locked bodies.
    ```

## Advanced Settings Definitions

  - **landscape**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Altitude of geographical features is multiplied by the "Resize" and "landscape" parameters.
    ```

  - **geeASLmultiplier**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Surface gravity is multiplied by this value.
    ```

  - **resizeScatter**, *\<double\>*, *default value = 1*, Disabled if set to zero.

    <pre>
    <b>ENABLED:</b>  ground scatter size is multiplied by the "Resize" parameter.
    <b>DISABLED:</b> ground scatter size is not modified.

    Ground scatter density is always adjusted to account for the different surface area.
    </pre>

  - **resizeBuildings**, *\<double\>*, *default value = 1*, Automatic when zero, custom when set to a positive number.

    <pre>
    <b>AUTOMATIC:</b> buildings size is multiplied by "Resize" when shrinking planets.
    <b>CUSTOM:</b>    buildings size is multiplied by this parameter.
    </pre>

  - **groundTiling**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Ground textures tiling is multiplied by this parameter.
    ```

  - **CustomSoISize**, *\<double\>*, *default value = 0*, Enabled if set to any positive number.

    <pre>
    <b>ENABLED:</b>  Sphere of Influence is multiplyed by this parameter.
    <b>DISABLED:</b> Sphere of Influence is multiplyed by the "Rescale" parameter.

    Affects only bodies with the Sphere of Influence defined in their config file.
    </pre>

  - **CustomRingSize**, *\<double\>*, *default value = 0*, Enabled if set to any positive number.

    <pre>
    <b>ENABLED:</b>  rings size is multiplyed by this parameter.
    <b>DISABLED:</b> rings size is multiplyed by the "Rescale" parameter.
    </pre>

  - **atmoASL**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Atmospheric pressure at surface level is multiplied by this parameter.
    ```

  - **tempASL**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Surface temperature is multiplied by this parameter.
    ```

  - **atmoTopLayer**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Atmosphere height is multiplied by this parameter. Atmosphere curves are extended/trimmed accordingly.
    ```

  - **atmoVisualEffect**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    The height of the atmosphere aesthetics is multiplied by this parameter.
    ```

  - **lightRange**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    The distance component of light intensity curves is multiplied by the "Rescale" and "lightRange" parameters.
    ```

  - **scanAltitude**, *\<double\>*, *default value = 1*, Can be set to any positive number.

    ```
    Altitude limits for orbital scanners is multiplied by the "Resize" and "scanAltitude" parameters.
    ```

  - **debug**, *\<bool\>*, *default value = false*
  
    <pre>
    When 'true' fills the file <i>output_log.txt</i> with debug information
    </pre>

## Planet Specific Changes

To apply Planet Specific Changes follow these instructions.

  **1.** Create a .cfg file with the following code in it

<pre>
@Kopernicus:BEFORE[SigDim2]:NEEDS[SigDim]
{
    @Body:HAS[#name[<i>PLANET_NAME_HERE</i>]]
    {
        @SigmaDimensions
        {
            @<i>PARAMETER</i> = <i>VALUE</i>
        }
    }
}</pre>

  **2.** Replace *```PLANET_NAME_HERE```* with the name of the planet you want to change

  **3.** Replace *```PARAMETER```* with the name of the parameter you want to overwrite

  **4.** Replace *```VALUE```* with the value you want to assign to the parameter

  **5.** You can edit as many parameters as you want

  **6.** Save the .cfg file anywhere in your KSP GameData folder

  **7.** **Do not mess this up. Everything will break.**

# PQSCity_Groups

```
PQSCity_Groups
{
}
```
This is the root node, you can add as many as you want, and they can be modified using
[ModuleManager](http://forum.kerbalspaceprogram.com/index.php?/topic/50533-0/) patches.

The PQSCity_Groups root node contains the groups definitions:

  - **GROUP**

    ```
    PQSCity_Groups
    {
        GROUP
        {
        }
    }
    ```
    This node is used to define a group, you can add as many GROUP nodes as you want.

    Every GROUP node contains four general settings and three nodes:

    - **name**, *\<string\>*, ***required***

      ```
      The name of the group
      ```

    - **body**, *\<string\>*, ***required***

      ```
      The name of the body on which the group is found
      ```

    - **debug**, *\<bool\>*, *default value = false*

    <pre>
    When 'true' fills the file <i>output_log.txt</i> with debug information
    </pre>

    <br>**NOTE:** Groups with the same name found on the same body will be merged and considered as one.<br><br><br>

    - **CENTER**

      ```
      PQSCity_Groups
      {
          GROUP
          {
              CENTER
              {
              }
          }
      }
      ```

      This node is used to define the center of the group.

      The center can be defined in many different ways, the first valid option will be chosen.

      - **CentralPQSCity**, *\<string\>*, the name of the central PQSCity mod
      - **CentralPQSCity2**, *\<string\>*, the name of the central PQSCity2 mod
      - **CentralPosition**, *\<Vector3\>*, the position of the center defined in a 3D space
      - **CentralLAT**, *\<double\>*, the latitude of the center (requires CentralLON)
      - **CentralLON**, *\<double\>*, the longitude of the center (requires CentralLAT)

      <br>**NOTE:**

      If the node CENTER is not defined, or the positions defined are not valid,
      the central position will be defined as the position of the first valid PQSMod of the group<br><br><br>

    - **MODS**

      ```
      PQSCity_Groups
      {
          GROUP
          {
              MODS
              {
              }
          }
      }
      ```

      This node is used to list all PQSMods included in the group.

      The PQSMods that are currently supported are:

      - **PQSCity**, *\<string\>*, the name of the PQSCity mod
      - **PQSCity2**, *\<string\>*, the name of the PQSCity2 mod
      <br><br><br>
    - **MOVE**

      ```
      PQSCity_Groups
      {
          GROUP
          {
              MOVE
              {
              }
          }
      }
      ```

      This node is used to define a new central position,

      all mods in the group will be moved around this new center

      The new center can be defined in many different ways, the first valid option will be chosen.

      - **CentralPQSCity**, *\<string\>*, the name of the new central PQSCity mod
      - **CentralPQSCity2**, *\<string\>*, the name of the new central PQSCity2 mod
      - **CentralPosition**, *\<Vector3\>*, the position of the new center defined in a 3D space
      - **CentralLAT**, *\<double\>*, the latitude of the new center (requires CentralLON)
      - **CentralLON**, *\<double\>*, the longitude of the new center (requires CentralLAT)

      In addition to the position of the new center, it is possible to define:

      - **Rotate**, *\<double\>*, angle in degrees, the whole group will be rotated around the center
      - **fixAltitude**, *\<double\>*, meters, added to the altitude of each mod in the group
      - **originalAltitude**, *\<double\>*, meters, needed if the base is designed for a different planet

      <br>**NOTE:**

      **1-** If the positions defined for the new center are not valid the group will not be affected

      **2-** Groups centered around the KSC should not be moved using this feature,
      <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;use the SpaceCenter feature from Kopernicus
