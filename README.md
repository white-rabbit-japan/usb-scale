# usb-scale

### Written in C# for .NET projects

Get Weight from USB Scales supports the follow type scales:
- DYMO M25 (25 lbs; 11kg)
- ELANE USB 30 (30kg)

You could add more scale types by adding the vendor/product hardware ids for the HID-compliant device

some more you could add it:

  // Stamps.com Model 510 5LB Scale
    {0x1446, 0x6a73},
    // USPS (Elane) PS311 "XM Elane Elane UParcel 30lb"
    {0x7b7c, 0x0100},
    // Stamps.com Stainless Steel 5 lb. Digital Scale
    {0x2474, 0x0550},
    // Stamps.com Stainless Steel 35 lb. Digital Scale
    {0x2474, 0x3550},
    // Mettler Toledo
    {0x0eb8, 0xf000},
    // SANFORD Dymo 10 lb USB Postal Scale
    {0x6096, 0x0158},
    // Fairbanks Scales SCB-R9000
    {0x0b67, 0x555e},
    // Dymo-CoStar Corp. M25 Digital Postal Scale
    {0x0922, 0x8004},
    // DYMO 1772057 Digital Postal Scale
    {0x0922, 0x8003}
    
