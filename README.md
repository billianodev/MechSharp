# MechSharp

[![ko-fi](https://img.shields.io/badge/Support_me_on-Ko--fi-red)](https://ko-fi.com/G2G1SRUJG)
[![](https://img.shields.io/badge/Built_with-Avalonia-purple)](https://avaloniaui.net/)
[![](https://img.shields.io/badge/Built_with-.NET-blue)](https://dot.net/)
[![](https://img.shields.io/badge/Check-Mechvibes-white)](https://mechvibes.com)

---

MechSharp is an app that simulate mechanical keyboard and is a rewrite of Mechvibes that brings key up, random sounds and aim to provides better performance

---

## How to use

1. Download it from releases
2. Extract
3. Run

_No need to install anything!_

### Migrating from Mechvibes or Mechvibes++

If you had use Mechvibes or Mechvibes++ before, MechSharp is designed to provides compatibility for both and most soundpack from both should be compatible with MechSharp for further info see the comparison part below.

---

## Comparison

### Performance

_Tested with_\
_OS: Windows 11_\
_CPU: Intel i7-8750H @ 2.2GHz 6 core_

|                | MechSharp   | Mechvibes  | Mechvibes++ |
|----------------|-------------|------------|-------------|
| Installer size | __40 MB__   | 70 MB      | 70 MB       |
| Actual size    | __< 90 MB__ | 160+ MB    | 160+ MB     |
| Memory usage   | __60 MB__   | 90-110+ MB | 300+ MB     |
| CPU usage      | __0-3%__    | 3-7%       | 3-10%       |

👑 __Overall MechSharp__

### Features

|                        | MechSharp             | Mechvibes         | Mechvibes++ |
|------------------------|-----------------------|-------------------|-------------|
| Supported platform     | __win ~~mac linux~~__ | __win mac linux__ | win         |
| Soundpack editor       | x                     | __v__             | __v__       |
| Soundpack fast install | x                     | __v__             | x           |
| Volume                 | __Up to 200__         | Up to 100         | Up to 100   |
| Volume step            | 5                     | 5                 | 1           |
| Key up sound           | __v__                 | x                 | __v__       |
| Random sound           | __v__                 | x                 | __v__       |
| Mousepack support      | __v__                 | x                 | __v__       |

👑 __Each have different sets of features depending on your needs, but I might say MechSharp__

_*Linux and MacOS is now in experimental! For now you can compile it yourself_

### Backend

|              | MechSharp     | Mechvibes     | Mechvibes++   |
|--------------|---------------|---------------|---------------|
| Audio api    | PortAudio     | Web Audio API | Web Audio API |
| Audio codecs | CSCore Codecs | Web Audio API | Web Audio API |
| Audio lib    | NAudio        | Howler        | Howler        |
| GUI backend  | Avalonia      | Electron      | Electron      |
| Key hook     | libuiohook    | iohook        | iohook        |

* _CSCore and Web Audio API provide collection of codecs which mean it utilize many other codecs such MediaFoundation (windows only) and much more_