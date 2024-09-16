<img src="Documentation/get_attribute.gif?raw=true" alt="Add Attribute Example" width="1027px" height="273px"/>

# Get Component Attribute
Attribute for auto-attach components in serialized fields. Searches for component of field type.

All attachments are made in the editor, no logic in runtime.

**!!! It attaches components in empty fields only! If field was already filled, it won't do anything !!!**

**!!! You need to open script in inspector for auto-fill fields !!!**

Supports multi-object editing.

**It doesn`t support attachment to arrays yet :(**

## Table Of Contents
<details>
<summary>Details</summary>
  
  - [Installation](#installation)
  - [How To Use](#how-to-use)
  - [Available Attributes](#available-attributes)

</details>

## Installation
 - Go to **Package Manager**
 - Press **+** and select **Add package from git URL...**

Paste following link in field:
```
https://github.com/Hidgal/Get-Component-Attribute.git
```

## How To Use
Just add one of available attributes to the serialized field and open it`s inspector.

Example:
```c#
using GetAttribute;

[GetInParent]
public Canvas ParentCanvas;

[GetInChildren]
[SerializeField]
private Image _icon;

[GetInObject]
[SerializeField]
private RectTransform _rectTranform;
```

## Available Attributes
- **[GetInObject]** - the same as GetComponent<>().
- **[GetInChildren]** or **[GetInChildren(bool includeInactive)]** - the same as GetComponentInChildren<>() and GetComponentInChildren<>(bool includeInactive).
- **[GetInParent]** - the same as GetComponentInParent<>().
      
