## Sigma Dimensions ##

# Forum Thread: http://forum.kerbalspaceprogram.com/index.php/topic/126548-/



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


# geeASLmultiplier (default value = 1)

- Can be set to any positive number.

Surface gravity is multiplied by this value.

---

# landscape (default value = 1)

- Can be set to any positive number.

Altitude of geographical features is multiplied by the "Resize" and "landscape" parameters.

---

# atmoVisualEffect (default value = 1)

- Can be set to any positive number.

The height of the atmosphere aesthetics is multiplied by this parameter.

---

# resizeScatter (default value = 1)

- Disabled if set to 0

ENABLED:  ground scatter size is multiplied by the "Resize" parameter.
DISABLED: ground scatter size is not modified.

Ground scatter density is always adjusted to account for the different surface area.

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
		@PlanetDimensions[id] = value
	}
}

//   END CODE   //


#02 - Replace 'PLANET_NAME_HERE' with the 'name' of the planet you want to change


#03 - Replace 'id' with the number of the parameter you want to overwrite

	List of parameters:
	
	[0]  - Resize
	[1]  - Rescale
	[2]  - Atmosphere
	[3]  - dayLengthMultiplier
	[4]  - geeASLmultiplier
	[5]  - landscape
	[6]  - atmoVisualEffect
	[7]  - resizeScatter
	[8]  - CustomSoISize
	[9]  - CustomRingSize
	[10] - atmoASL
	[11] - tempASL

	Example: (Set to '0.5' the 'Atmosphere' parameter for Kerbin)

	//  START CODE  //

	@Kopernicus:BEFORE[SigDim2]:NEEDS[SigDim]
	{
		@Body:HAS[#name[Kerbin]]
		{
			@PlanetDimensions[2] = 0.5
		}
	}

	//   END CODE   //


#04 - To edit another parameter paste another copy of the code and edit it accordingly


#05 - Save the .cfg file anywhere in your KSP GameData folder.


#06 - Do not mess this up. Everything will break.



## For other questions, visit the Forum Thread:
# http://forum.kerbalspaceprogram.com/index.php/topic/126548-/



## Sigma88 ##
