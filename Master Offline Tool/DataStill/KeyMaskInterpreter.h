#pragma once

#include <math.h>
#include <vector>
#include <string>
// Needs to be the same as the one in mortal online. There are 2 because 1 couldnt fit all input and UC didnt have int64
enum Key1BitMask:int
{
    A = 0x1,
    B = 0x2,
    C = 0x4,
    D = 0x8,
    E = 0x10,
    F = 0x20,
    G = 0x40,
    H = 0x80,
    I = 0x100,
    J = 0x200,
    K = 0x400,
    L = 0x800,
    M = 0x1000,
    N = 0x2000,
    O = 0x4000,
    P = 0x8000,
    Q = 0x10000,
    R = 0x20000,
    S = 0x40000,
    T = 0x80000,
    U = 0x100000,
    V = 0x200000,
    W = 0x400000,
    X = 0x800000,
    Y = 0x1000000,
    Z = 0x2000000,
    Tab = 0x4000000,
    CapsLock = 0x8000000,
    LeftShift = 0x10000000,
    LeftControl = 0x20000000,
    LeftAlt = 0x40000000,
};
enum Key2BitMask: int
{
    Zero = 0x1,
    One = 0x2,
    Two = 0x4,
    Three = 0x8,
    Four = 0x10,
    Five = 0x20,
    Six = 0x40,
    Seven = 0x80,
    Eight = 0x100,
    Nine = 0x200,
    LeftMouseButton = 0x400,
    RightMouseButton = 0x800,
    SpaceBar = 0x1000,
    Escapea = 0x2000,
    Tilde = 0x4000,
    BackSpace = 0x8000,
    Enter = 0x10000,
    RightControl = 0x20000,
    RightShift = 0x40000,
    F1 = 0x80000,
    F2 = 0x100000,
    F3 = 0x200000,
    F4 = 0x400000,
    F5 = 0x800000,
    F6 = 0x1000000,
    F7 = 0x2000000,
    F8 = 0x4000000,
    F9 = 0x8000000,
    F10 = 0x10000000,
    F11 = 0x20000000,
    F12 = 0x40000000,
};

class KeyMaskInterpreter
{
public:
    KeyMaskInterpreter();
    ~KeyMaskInterpreter();
    /**
    Takes all the raw data from a key logged raw data file and and makes it so that each key gets its own int instead of being
    represented in the bit mask
    */
    void ReinterpretRawKeyData(std::vector<std::string>* o_lines);
};

