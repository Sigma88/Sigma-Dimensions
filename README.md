# Sigma Dimensions


**The Universal Rescale Mod for KSP**


KSP Forum Thread: http://forum.kerbalspaceprogram.com/index.php?/topic/126548-0/

Download Latest Release: https://github.com/Sigma88/Sigma-Dimensions/releases/latest

Dev version: https://github.com/Sigma88/Sigma-Dimensions/tree/Development


# Settings

## Base Settings Definitions

  - **Resize &nbsp; ```(default value = 1)```** &nbsp; ```Can be set to any positive number.```
    
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
