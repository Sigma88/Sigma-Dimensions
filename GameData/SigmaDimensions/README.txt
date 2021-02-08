## Sigma Dimensions ##

# GitHub Repository: https://www.github.com/Sigma88/Sigma-Dimensions



## Base Settings Definitions


# Resize (default value = 1)

- Can be set to any positive number.

Planetary Radius is multiplied by this value.

The mass of each body is changed to maintain the same surface gravity.

---

# Rescale (default value = 1)

- Can be set to any positive number.

Orbit size (SemiMajor Axis) is multiplied by this value.

---

# Atmosphere (default value = 1)

- Can be set to any positive number.

Atmosphere height is multiplied by this value.

---

# dayLengthMultiplier (default value = 1)

- Can be set to any positive number.

Rotation period is multiplied by this value.

Does not affect tidally locked bodies.

---




## Advanced Settings Definitions


# landscape (default value = 1)

- Can be set to any positive number.

Altitude of geographical features is multiplied by the "Resize" and "landscape" parameters.

---

# geeASLmultiplier (default value = 1)

- Can be set to any positive number.

Surface gravity is multiplied by this value.

---

# resizeScatter (default value = 1)

- Disabled if set to 0

ENABLED:  ground scatter size is multiplied by the "Resize" parameter.
DISABLED: ground scatter size is not modified.

Ground scatter density is always adjusted to account for the different surface area.

---

# resizeBuildings (default value = 0)

- Automatic when zero, custom when set to a positive number.

AUTOMATIC: buildings size is multiplied by "Resize" when shrinking planets.
CUSTOM:    buildings size is multiplied by this parameter.

---

# groundTiling (default value = 1)

- Can be set to any positive number.

Ground textures tiling is multiplied by this parameter.

---

# CustomSoISize (default value = 0)

- Enabled if set to a positive number.

ENABLED:  Sphere of Influence is multiplyed by this parameter.
DISABLED: Sphere of Influence is multiplyed by the "Rescale" parameter.

Affects only bodies with the Sphere of Influence defined in their config file.

---

# CustomRingSize (default value = 0)

- Enabled if set to a positive number.

ENABLED:  rings size is multiplyed by this parameter.
DISABLED: rings size is multiplyed by the "Rescale" parameter.

---

# atmoASL (default value = 1)

- Can be set to any positive number

Atmospheric pressure at surface level is multiplied by this parameter.

---

# tempASL (default value = 1)

- Can be set to any positive number.

Surface temperature is multiplied by this parameter.

---

# atmoTopLayer (default value = 1)

- Can be set to any positive number.

Atmosphere height is multiplied by this parameter. Atmosphere curves are extended/trimmed accordingly.

---

# atmoVisualEffect (default value = 1)

- Can be set to any positive number.

The height of the atmosphere aesthetics is multiplied by this parameter.

---

# lightRange (default value = 1)

- Can be set to any positive number.

The distance component of light intensity curves is multiplied by the "Rescale" and "lightRange" parameters.

---

# scanAltitude (default value = 1)

- Can be set to any positive number.

Altitude limits for orbital scanners is multiplied by the "Resize" and "scanAltitude" parameters.

---




## Planet Specific Changes


To apply Planet Specific Changes follow these instructions.


#01 - create a .cfg file with the following code in it

//  START CODE  //

@Kopernicus:BEFORE[SigDim2]:NEEDS[SigDim]
{
	@Body:HAS[#name[PLANET_NAME_HERE]]
	{
		@SigmaDimensions
		{
			@PARAMETER = VALUE
		}
	}
}

//   END CODE   //


#02 - Replace 'PLANET_NAME_HERE' with the 'name' of the planet you want to change


#03 - Replace 'PARAMETER' with the name of the parameter you want to overwrite

	Example: (Set to '0.5' the 'Atmosphere' parameter for Kerbin)

	//  START CODE  //

	@Kopernicus:BEFORE[SigDim2]:NEEDS[SigDim]
	{
		@Body:HAS[#name[Kerbin]]
		{
			@SigmaDimensions
			{
				@Atmosphere = 0.5
			}
		}
	}

	//   END CODE   //


#04 - You can edit as many parameters as you want


#05 - Save the .cfg file anywhere in your KSP GameData folder.


#06 - Do not mess this up. Everything will break.



## For other questions, visit the GitHub Repository:
# https://www.github.com/Sigma88/Sigma-Dimensions



## Sigma88 ##
