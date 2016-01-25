## Sigma Dimensions ##

# Forum Thread: http://forum.kerbalspaceprogram.com/index.php/topic/126548-/



## Base Settings Definitions


# Resize (default value = 1)

- Can be set to any number >0

Each celestial body size will be multiplied by this value.

Mass of each body will be changed in order to maintain the same gravity at surface level

---

# Rescale (default value = 1)

- Can be set to any number >0

Orbit size (SemiMajor Axis) will be multiplied by this value.

---

# Atmosphere (default value = 1)

- Can be set to any number >0

Height of the Atmosphere on each body will be multiplied by this value.

---

# dayLengthMultiplier (default value = 1)

- Can be set to any number >0

Rotation period of each body will be multiplied by this value.
Doesn't affect tidally locked bodies.

---




## Advanced Settings Definitions


# geeASLmultiplier (default value = 1)

- Can be set to any number >0

Gravity at surface level will be multiplied by this value.
Mass of each body will be changed in order to obtain the correct result.

---

# landscape (default value = 1)

- Can be set to any number >0

Altitude of geographical features will be first multiplied by the "Resize" parameter and then by the "landscape" parameter.

---

# atmoVisualEffect (default value = 1)

- Can be set to any number >0

The height of the atmosphere visual effect will be multiplied by this parameter.

---

# resizeScatter (default value = 1)

- Disabled only if set to 0

If enabled, scatter size will be multiplied by the "Resize" parameter.

---

# CustomSoISize (default value = 1)

- Can be set to any number >0

Affects only bodies with Sphere of Influence defined in their config file.

By default, SoI size is multiplied by the "Rescale" parameter.

When "CustomSoISize" is enabled, SoI size is multiplyed by this parameter instead.

---

# CustomRingSize (default value = 1)

- Can be set to any number >0

By default, rings size is multiplied by the "Rescale" parameter.

When "CustomRingSize" is enabled, rings size is multiplyed by this parameter instead.

---

# atmoASL (default value = 1)

- Can be set to any number >0

Atmospheric pressure at surface level is multiplied by this parameter.

---

# tempASL (default value = 1)

- Can be set to any number >0

Atmospheric temperature at surface level is multiplied by this parameter.

---




## Planet Specific Changes


In order to apply Planet Specific Changes you need to:


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

	@Kopernicus:BEFORE[SigDim]:NEEDS[SigDim]
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
