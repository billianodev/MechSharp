# MechSharp

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/G2G1SRUJG)
[![](https://img.shields.io/badge/Built_with-Avalonia-blue)](https://avaloniaui.net/)
[![](https://img.shields.io/badge/Built_with-.NET-blue)](https://dot.net/)
[![](https://img.shields.io/badge/Check-Mechvibes-blue)](https://mechvibes.com)

---

MechSharp is an app that simulate mechanical keyboard and is a rewrite of Mechvibes that brings key up, random sounds and aim to provides better performance

Even though MechSharp is programmed using .NET C# which not really the best for performance, most release might have AOT label in which the performance is comparable to other native programming language such as C, C++.

---

## How to use

1. Download it from releases
1. Extract
1. Run

_No need to install anything!_

### Migrating from Mechvibes or Mechvibes++

If you had use Mechvibes or Mechvibes++ before, MechSharp is designed to provides compatibility for both and most soundpack from both should be compatible with MechSharp for further info see the comparison part below.

---

## Comparison

### Performance

_Tested with my Windows 11 laptop_
_CPU: Intel i7-8750H @ 2.2GHz 6 core_
_RAM: 16 GB_

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|Intaller size|__30+ MB__|70 MB|70 MB|
|Actual size|__55-80 MB__|160+ MB|160+ MB|
|Memory usage|__50-70 MB__|90-110+ MB|300+ MB|
|CPU usage|__< 1%__|3-7%|3-10%|

👑 __Overall MechSharp__

### Features

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|Supported platform|win|__win mac linux__|win|
|Soundpack editor|x|__v__|__v__|
|Soundpack fast install|x|__v__|x|
|Volume|__Up to 200__|Up to 100|Up to 100|
|Volume step|5|5|1|
|Key up sound|__v__|x|__v__|
|Random sound|__v__|x|__v__|
|Mousepack support|__v__|x|__v__|

👑 __Overall about the same__

_*Linux and MacOS is technically supported but have many issue, so compile it yourself and modify as nessesary_

### Backend

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|Audio api|PortAudio|Web Audio API|Web Audio API|
|Audio codecs|Media Foundation|Web Audio API|Web Audio API|
|Audio lib|NAudio|Howler|Howler|
|GUI backend|Avalonia|Electron|Electron|
|Key hook|libuiohook|iohook|iohook|

_*Since MechSharp use Media Foundation as codecs it mean that if you target platform other than Windows most audio format will not be compatible and will only support wav, mp3 and ogg_