## Sigma Dimensions ##


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

# orbitalPeriod (default value = 0)

- Enabled only if set to any number >0

Orbital period of each body will be multiplied by this value.
Recalculates the "Rescale" parameter in order to obtain the correct result.

---

# daysVSyearRatio (default value = 0)

- Enabled only if set to any number >0

The ratio [RotationPeriod]/[OrbitalPeriod] will be multiplied by this value.
Recalculates the "dayLenghtMultiplier" parameter in order to obtain the correct result.

---

# resizeScatter (default value = 1)

- Disabled only if set to 0

If enabled, scatter size will be multiplied by the "Resize" parameter.

---

# landscape (default value = 1)

- Enabled only if set to any number >0

Altitude of geographical features will be first multiplied by the "Resize" parameter and then by the "landscape" parameter.

---

# atmoVisualEffect (default value = 1)

- Enabled only if set to any number >0

The height of the atmosphere visual effect will be multiplied by this parameter.

---



## For other questions, visit the Forum Thread:
# http://forum.kerbalspaceprogram.com/index.php/topic/126548-/



## Sigma88 ##
