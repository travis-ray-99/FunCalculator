# 🍬 Fun Calculator 🍬

A colorful, whimsical desktop calculator built with **.NET 10** and **Avalonia UI**.

![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)
![Avalonia UI](https://img.shields.io/badge/Avalonia-11.3-blue)

## ✨ Features

- **Candy-gradient background** — a vibrant purple → blue → pink → teal gradient
- **Frosted-glass number buttons** with semi-transparent overlays
- **Bouncy press & hover animations** — buttons scale up on hover and shrink on press
- **Fun emoji accents** — 🧹 Clear, ✨ Equals, 🤯 division-by-zero message
- **Full calculator operations** — addition, subtraction, multiplication, division, negate (±), and percent (%)
- **MVVM architecture** — clean separation of `CalculatorEngine`, `CalculatorViewModel`, and XAML views

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run

```bash
dotnet run --project src/FunCalculator
```

### Build

```bash
dotnet build FunCalculator.slnx
```

### Test

```bash
dotnet test FunCalculator.slnx
```

## 🏗️ Project Structure

```
FunCalculator/
├── FunCalculator.slnx              # Solution file
├── src/FunCalculator/
│   ├── Models/
│   │   └── CalculatorEngine.cs     # Pure arithmetic logic
│   ├── ViewModels/
│   │   ├── CalculatorViewModel.cs  # MVVM ViewModel with commands
│   │   └── RelayCommand.cs         # ICommand implementation
│   ├── App.axaml / App.axaml.cs    # Application entry & theme
│   ├── MainWindow.axaml / .cs      # The colorful calculator UI
│   └── Program.cs                  # Desktop bootstrap
└── tests/FunCalculator.Tests/
    ├── CalculatorEngineTests.cs     # Engine unit tests
    └── CalculatorViewModelTests.cs  # ViewModel unit tests
```

## 🎨 Design

The calculator uses a **candy theme** with:

| Element | Style |
|---------|-------|
| Background | Purple → Blue → Pink → Teal gradient |
| Number buttons | Frosted semi-transparent white |
| Operator buttons | Coral red |
| Function buttons | Lavender purple |
| Equals button | Golden amber |
| Display | Dark glass overlay with glowing white text |
