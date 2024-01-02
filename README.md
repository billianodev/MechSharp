# MechSharp

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/G2G1SRUJG)
[![](https://img.shields.io/badge/Built_with-Avalonia-blue)](https://avaloniaui.net/)
[![](https://img.shields.io/badge/Built_with-.NET-blue)](https://dot.net/)
[![](https://img.shields.io/badge/Check-Mechvibes-blue)](https://mechvibes.com)

---

MechSharp is an app that simulate mechanical\
keyboard and is a rewrite of Mechvibes\
that brings key up, random sounds\
and aim to provides better performance

Even though MechSharp is programmed using .NET C#\
which not really the best for performance,\
most release might have AOT tag\
in which the performance is comparable to other\
native programming language such as C, C++.

---

## How to use

1. Download it from releases
1. Extract
1. Run

_No need to install anything!_

### Migrating from Mechvibes or Mechvibes++

If you had use Mechvibes or Mechvibes++ before,\
MechSharp is designed to provides compatibility for both and\
most soundpack from both should be compatible with MechSharp\
for further info see the comparison part below.

---

## Comparison

### Device

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|Supported platform|win|win mac linux|win mac linux|
|Intaller size|30+ MB|70 MB|70 MB|
|Actual size|55-80 MB|160+ MB|160+ MB|
|Memory usage|50-70 MB|90-110+ MB|300+ MB|
|CPU usage|< 1%|3-7%|3-10%|

_*Performance are tested with my Windows 11 device_
_*Linux and MacOS is technically supported but have many issue_

### Features

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|Soundpack editor|x|v|v|
|Supported audio format|wav mp3 ogg|15+|15+|
|Volume|Up to 200|Up to 100|Up to 100|
|Volume step|5|5|1|
|Key up sound|v|x|v|
|Random sound|v|x|v|
|Mousepack support|v|x|v|

### Backend

||MechSharp|Mechvibes|Mechvibes++|
|-|-|-|-|
|GUI backend|Avalonia|Electron|Electron|
|Key hook|libuiohook|libuiohook|libuiohook|
|Audio lib|NAudio|Howler|Howler|
|Audio api|PortAudio|Web Audio API|Web Audio API|