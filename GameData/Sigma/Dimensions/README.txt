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

- Enabled only if set to any number >0

Altitude of geographical features will be first multiplied by the "Resize" parameter and then by the "landscape" parameter.

---

# atmoVisualEffect (default value = 1)

- Enabled only if set to any number >0

The height of the atmosphere visual effect will be multiplied by this parameter.

---

# resizeScatter (default value = 1)

- Disabled only if set to 0

If enabled, scatter size will be multiplied by the "Resize" parameter.

---

# SoIsFromRadius (default value = 0)

- Can only be set to 0 or 1

Affects only bodies with Sphere of Influence defined in their config file.

If 0 - Multiplies the sphere of influence size by "Rescale"
If 1 - Multiplies the sphere of influence size by "Resize"

---

# RingsFromRadius (default value = 0)

- Can only be set to 0 or 1

Affects only bodies with rings

If 0 - Multiplies the rings size by "Rescale"
If 1 - Multiplies the rings size by "Resize"

---




## Planet Specific Changes


In order to apply Planet Specific Changes you need to:


#01 - create a .cfg file with the following code in it

//  START CODE  //

@Kopernicus:BEFORE[SigDim]:NEEDS[SigDim]
{
	@Body:HAS[#name[PLANET_NAME_HERE]]
	{
		%PlanetDimensions = Resize,Rescale,Atmosphere,dayLengthMultiplier,geeASLmultiplier,landscape,atmoVisualEffect,resizeScatter,SoIsFromRadius,RingsFromRadius
	}
}

//   END CODE   //


#02 - Replace 'PLANET_NAME_HERE' with the 'name' of the planet you want to change


#03 - Replace the name of the parameter you want to change with the value you want

	Example: (Set to '0.5' the 'Atmosphere' parameter for Kerbin)

	//  START CODE  //

	@Kopernicus:BEFORE[SigDim]:NEEDS[SigDim]
	{
		@Body:HAS[#name[Kerbin]]
		{
			%PlanetDimensions = Resize,Rescale,0.5,dayLengthMultiplier,geeASLmultiplier,landscape,atmoVisualEffect,resizeScatter,SoIsFromRadius,RingsFromRadius
		}
	}

	//   END CODE   //


#04 - To edit another planet paste another copy of the code and edit it accordingly


#05 - Save the .cfg file anywhere in your KSP GameData folder.




## For other questions, visit the Forum Thread:
# http://forum.kerbalspaceprogram.com/index.php/topic/126548-/



## Sigma88 ##
