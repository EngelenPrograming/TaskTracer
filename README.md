# 🧭 Handy for Project Managers
TaskTracer automatically scans your entire codebase and collects all TODO comments — no IDE required. This makes it an excellent tool for project managers who want a quick, high-level overview of unfinished tasks or pending improvements directly from the source code, helping bridge the gap between developers and management.


**TaskTracer** is a lightweight desktop tool built with **Avalonia** and **ReactiveUI** that scans your source code for `TODO` comments and organizes them in one place.  
It’s perfect for developers who want to quickly find unfinished tasks or reminders scattered throughout their codebase.


![TaskTracer Screenshot](TaskTrace/images/program.png)

## Small donations
in case you find this handy or you would like to support my developing career.
- https://www.paypal.com/donate/?hosted_button_id=FGZYCX4Q4BNN6

---

## ✨ Features

- 🔍 **Scan any folder** for TODO comments across multiple files.  
- 📂 **Browse folders** easily using a native folder picker.  
- 📝 **View and export** all TODOs to a JSON file.  
- 💾 **Remembers your last used folder** between sessions.  
- ⚡ Built with **.NET + Avalonia**, runs on Windows, macOS, and Linux.

---

## Quick start:
download the Exe from the releases Tab on Github

## 🚀 Compiling the code yourself

### 1. Clone the repository
```bash
git clone https://github.com/engelen305-gif/TaskTracer.git
```

### 2. build the project
in the project root do the following:
```bash

dotnet restore TaskTrace.sln
dotnet build TaskTrace.sln --configuration Release
dotnet run --project ./TaskTrace/TaskTrace.csproj
```


