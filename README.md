# Sigma Dimensions


**The Universal Rescale Mod for KSP**


KSP Forum Thread: http://forum.kerbalspaceprogram.com/index.php?/topic/126548-0/

Download Latest Release: https://github.com/Sigma88/Sigma-Dimensions/releases/latest

Dev version: https://github.com/Sigma88/Sigma-Dimensions/tree/Development


# Settings

## Base Settings Definitions

  - **Resize**, *\<double\>*, *```default value = 1```*, &nbsp; ```Can be set to any positive number```.
    
    ```
    Planetary Radius is multiplied by this value.

    The mass of each body is changed to maintain the same surface gravity.
    ```
    
  - **Rescale &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```
		
    ```
    Orbit size (SemiMajor Axis) is multiplied by this value.
    ```
    
  - **Atmosphere &nbsp; ```(default value = 1)```**&nbsp; ```Can be set to any positive number.```
		
    ```
    Atmosphere height is multiplied by this value.
    ```
    
  - **dayLengthMultiplier &nbsp; ```(default value = 1)```**&nbsp; ```Can be set to any positive number.```
		
    ```
    Rotation period is multiplied by this value.

    Does not affect tidally locked bodies.
    ```
    
## Advanced Settings Definitions

  - **landscape &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Altitude of geographical features is multiplied by the "Resize" and "landscape" parameters.
    ```

  - **geeASLmultiplier &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Surface gravity is multiplied by this value.
    ```

  - **resizeScatter &nbsp; ```(default value = 1)```** &nbsp; ```Disabled if set to 0```

    ```
    ENABLED:  ground scatter size is multiplied by the "Resize" parameter.
    DISABLED: ground scatter size is not modified.

    Ground scatter density is always adjusted to account for the different surface area.
    ```

  - **resizeBuildings &nbsp; ```(default value = 0)```** &nbsp; ```Automatic when zero, custom when set to a positive number.```

    ```
    AUTOMATIC: buildings size is multiplied by "Resize" when shrinking planets.
    CUSTOM:    buildings size is multiplied by this parameter.
    ```

  - **groundTiling &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Ground textures tiling is multiplied by this parameter.
    ```

  - **CustomSoISize &nbsp; ```(default value = 0)```** &nbsp; ```Enabled if set to a positive number.```

    ```
    ENABLED:  Sphere of Influence is multiplyed by this parameter.
    DISABLED: Sphere of Influence is multiplyed by the "Rescale" parameter.

    Affects only bodies with the Sphere of Influence defined in their config file.
    ```

  - **CustomRingSize &nbsp; ```(default value = 0)```** &nbsp; ```Enabled if set to a positive number.```

    ```
    ENABLED:  rings size is multiplyed by this parameter.
    DISABLED: rings size is multiplyed by the "Rescale" parameter.
    ```

  - **atmoASL &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Atmospheric pressure at surface level is multiplied by this parameter.
    ```

  - **tempASL &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Surface temperature is multiplied by this parameter.
    ```

  - **atmoTopLayer &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Atmosphere height is multiplied by this parameter. Atmosphere curves are extended/trimmed accordingly.
    ```

  - **atmoVisualEffect &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    The height of the atmosphere aesthetics is multiplied by this parameter.
    ```

  - **scanAltitude &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```

    ```
    Altitude limits for orbital scanners is multiplied by the "Resize" and "scanAltitude" parameters.
    ```

## PQSCity_Groups

```
PQSCity_Groups
{
}
```
This is the root node, you can add as many as you want, and it can be modified using [ModuleManager](http://forum.kerbalspaceprogram.com/index.php?/topic/50533-0/) patches.

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

    - **name**

      *String, required*
      ```
      The name of the group
      ```
      
    - **body**

      *String, required*
      ```
      The name of the body on which the group is found
      ```
      
    - **debug**

      *Boolean, default value = false*
      ```
      When 'true' fills the output_log.txt file with debug information
      ```
    
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
      
      **NOTE:**

      1- If the positions defined for the new center are not valid the group will not be affected

      2- Groups centered around the KSC should not be moved using this feature,
      <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;use the SpaceCenter feature from Kopernicus
      
      
