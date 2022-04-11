using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet 
{
    string petFamily;
    string petName;
    string colorCode;

    public Pet(string _petFamily, string _petname, string _colorCode)
    {
        petFamily = _petFamily;
        petName = _petname;
        colorCode = _colorCode;
    }
}
