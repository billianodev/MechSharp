# MechSharp

[![ko-fi](https://img.shields.io/badge/Support_me_on-Ko--fi-red)](https://ko-fi.com/G2G1SRUJG)
[![](https://img.shields.io/badge/Built_with-Avalonia-purple)](https://avaloniaui.net/)
[![](https://img.shields.io/badge/Built_with-.NET-blue)](https://dot.net/)
[![](https://img.shields.io/badge/Check-Mechvibes-white)](https://mechvibes.com)

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

Tested while GUI is open and spamming keys

OS: Windows 11 \
CPU: Intel i7-8750H @ 2.2GHz 6 core

#### General

|                | MechSharp     | Mechvibes | Mechvibes++ |
|----------------|---------------|-----------|-------------|
| Installer size | __40 MB__     | 70 MB     | 70 MB       |
| Actual size    | __90 MB__     | 160+ MB   | 160+ MB     |

#### Without mousepack

|                | MechSharp    | Mechvibes |
|----------------|--------------|-----------|
| Memory usage   | __55-65 MB__ | 90-110 MB |
| CPU usage      | __0-1%__     | 3-7%      |

#### With mousepack

|                | MechSharp    | Mechvibes++ |
|----------------|--------------|-------------|
| Memory usage   | __55-65 MB__ | 300+ MB     |
| CPU usage      | __0-1%__     | 3-10%       |

__👑 MechSharp__

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

__👑 MechSharp for general use, Mechvibes or Mechvibes++ for creating your own soundpack__

### Backend

|              | MechSharp              | Mechvibes     | Mechvibes++   |
|--------------|------------------------|---------------|---------------|
| Audio api    | PortAudio              | Web Audio API | Web Audio API |
| Audio codecs | NAudio & CSCore        | Web Audio API | Web Audio API |
| Audio lib    | NAudio & CSCore        | Howler        | Howler        |
| GUI backend  | Avalonia               | Electron      | Electron      |
| Key hook     | libuiohook             | iohook        | iohook        |


## Cross-platform Compatibility

_* Linux and MacOS is now in experimental! For now you can compile it yourself_