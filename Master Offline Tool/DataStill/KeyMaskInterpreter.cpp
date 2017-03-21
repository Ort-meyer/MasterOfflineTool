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
    for (size_t lineToRead = 1; lineToRead < o_lines->size(); lineToRead += 3)
    {
        std::stringstream in(o_lines->at(lineToRead));
        std::string finalStringPart1;
        std::string finalStringPart2;
        int mask1;
        int mask2;
        in >> mask1;
        in >> mask2;

        std::string index = o_lines->at(lineToRead - 1);
        // Make sure the index is after eachother
        if (lineToRead + 2 < o_lines->size())
        {
            std::string nextIndex = o_lines->at(lineToRead + 2);
            if (std::stoi(index) + 1 != std::stoi(nextIndex))
            {
                // Not sequential, one index have been removed
                o_lines->erase(o_lines->begin() + lineToRead - 1, o_lines->begin() + lineToRead + 2);
                lineToRead -= 3;
                continue;
            }
        }    
        
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
