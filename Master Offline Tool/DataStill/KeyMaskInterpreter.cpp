#include "KeyMaskInterpreter.h"
#include <sstream>


KeyMaskInterpreter::KeyMaskInterpreter()
{
    
}


KeyMaskInterpreter::~KeyMaskInterpreter()
{
}

void KeyMaskInterpreter::ReinterpretRawKeyData(std::vector<std::string>* o_lines)
{
    size_t length = o_lines->size();
    for (size_t lineToRead = 1; lineToRead < length; lineToRead +=3)
    {
        std::stringstream in(o_lines->at(lineToRead));
        std::string finalStringPart1;
        std::string finalStringPart2;
        int mask1;
        int mask2;
        in >> mask1;
        in >> mask2;

        // Hax thing so we can skip the enums with bitmasks
        // We loop over the number of changable bits in a int32
        for (size_t power = 0; power < 32; power++)
        {
            int ToThePowerOfpower = (int)(pow(2.0f, (float)power) + 0.5f);
            // We compare the mask1 with pow(2,i)
            if ((mask1 & ToThePowerOfpower) == ToThePowerOfpower)
            {
                // found a hit!
                finalStringPart1 += "1 ";
            }
            else
            {
                // The tested combo did not result in a hit
                finalStringPart1 += "0 ";
            }
            // We compare the mask2 with pow(2,i)
            if ((mask2 & ToThePowerOfpower) == ToThePowerOfpower)
            {
                // found a hit!
                finalStringPart2 += "1 ";
            }
            else
            {
                // The tested combo did not result in a hit
                finalStringPart2 += "0 ";
            }
        }
        o_lines->at(lineToRead) = finalStringPart1 + finalStringPart2;
    }
}
