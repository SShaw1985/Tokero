# Tokero - DCA Cryptocurrency Investment App

A cross-platform mobile application built with .NET MAUI that helps users configure and analyze Dollar Cost Averaging strategies for cryptocurrency investments.


## Features

### Authentication System
- **Login Page**: Secure authentication with email validation
- **Session Management**: Maintains authentication state throughout the app session

### Cryptocurrency Selection
- **Multi-Coin Selection**: Choose from 50+ popular cryptocurrencies including BTC, ETH, SOL, XRP, ADA, DOT, LINK, UNI, AVAX, MATIC, ATOM, NEAR, FTM, ALGO, VET, THETA, FIL, TRX, EOS, and AAVE
- **Search Functionality**: Filter cryptocurrencies by name for easy selection

### DCA Strategy Configuration
- **Investment Amount**: Set monthly investment amount in EUR (default: €200)
- **Investment Frequency**: Choose investment day of the month (1-28)
- **Start Date**: Select when to begin the DCA strategy (default: 1 year ago)
- **Portfolio Allocation**: Distribute investments across multiple cryptocurrencies

### Historical Price Analysis
- **Real-time Data**: Integrates with CoinMarketCap API for current cryptocurrency prices
- **Historical Pricing**: Simulates historical prices
- **Price Simulation**: Generates historical prices based on base prices for each cryptocurrency

### Portfolio Performance Tracking
- **Investment Records**: Detailed monthly investment tracking with dates, amounts, and coin quantities
- **Portfolio Valuation**: Current portfolio value calculation based on today's prices
- **ROI Analysis**: Individual and overall Return on Investment calculations
- **Performance Metrics**: Total invested amount, current portfolio value, and overall ROI percentage

### Data Visualization
- **Portfolio Value Chart**: Line chart showing portfolio value growth over time
- **ROI Performance Chart**: Monthly ROI performance visualization

### Results Display
- **Summary Cards**: Quick overview of total invested, portfolio value, and overall ROI
- **Detailed Table**: Monthly investment breakdown showing:
  - Investment date
  - Cryptocurrency symbol
  - Amount invested
  - Coin quantity purchased
  - Current value
  - Individual ROI percentage
- **Color-coded ROI**: Visual indicators for positive (green) and negative (red) returns

### Technical Features
- **Cross-platform Support**: Android, iOS, macOS, Windows, and Tizen
- **MVVM Architecture**: Clean separation of concerns with CommunityToolkit.Mvvm
- **Dependency Injection**: Service-based architecture for maintainability
- **Caching System**: Efficient data caching for improved performance
- **Error Handling**: Comprehensive error handling with user-friendly messages
- **Loading States**: Visual feedback during data operations

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or Visual Studio Code
- MAUI workload installed

### Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run on your preferred platform

### Release Build
There is a compild android APK located in the Build folder of this project
to install it copy onto an android device and click the apk file to install
follow the onscreen instructions to install 

### Login Instructions
**Important**: The app uses a simplified authentication system for demonstration purposes.

- **Email**: Enter any valid email format (e.g., `user@example.com`, `test@company.org`)
- **Password**: Enter any password (e.g., `password`, `123456`, `test`)
- **Validation**: The app validates email format but accepts any password


## Project Structure

```
Tokero/
├── Views/                # UI pages and popups
├── ViewModels/           # Business logic and data binding
├── Models/               # Data models and DTOs
├── Services/             # Business services and API integration
├── Interfaces/           # Service contracts
├── Converters/           # XAML value converters
├── Helpers/              # Utility classes
└── Platforms/            # Platform-specific implementations
└── Build/                # A release build of the app
```

## Dependencies

- **Microsoft.Maui.Controls**: Core MAUI framework
- **CommunityToolkit.Mvvm**: MVVM toolkit for data binding
- **CommunityToolkit.Maui**: MAUI-specific UI components
- **Microcharts.Maui**: Chart visualization library
- **Fody**: Code weaving for property change notifications
- **Akavache**: Caching library for improved performance

## API Integration

The app integrates with CoinMarketCap API for cryptocurrency data:
- **Base URL**: https://pro-api.coinmarketcap.com/v1
- **Features**: Current prices, cryptocurrency listings
- **Caching**: Implements caching layer for improved performance and reduced API calls

## Screenshots
![1-home](https://github.com/user-attachments/assets/f8d84c89-98c1-4f81-938e-4d9509adcdbb)
![2 -search](https://github.com/user-attachments/assets/eb6e93e1-d5f2-4ee7-95b9-56dceeb703e9)
![3 - coins](https://github.com/user-attachments/assets/7cb90255-7350-4127-b2ba-8ea8ece4affc)
![4 - filters](https://github.com/user-attachments/assets/ff878569-08ea-4104-a824-c3595c59c28e)
![5 - Results](https://github.com/user-attachments/assets/063a3924-a1ab-4823-8029-62581e0e0e3d)
![6 - Results 2](https://github.com/user-attachments/assets/e45eeab6-cc03-4808-a33d-803394bc19e6)
![7 - Results 3](https://github.com/user-attachments/assets/6ea09aed-3a8c-4a49-8f27-87643e98ac57)

