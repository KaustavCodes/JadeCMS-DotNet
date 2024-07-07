# JadedCms-DotNet

Welcome to **JadedCms-DotNet**, a cutting-edge Content Management System (CMS) built using .NET 8. This project is currently in development and aims to provide a flexible and robust platform for managing your content. JadedCms-DotNet is designed with a plugin architecture, allowing developers to extend and customize its functionality with ease.

## Features

- **Plugin Architecture**: Easily extend and customize the CMS functionality through plugins.
- **Initial Database Support**: Supports MySQL out of the box.
- **Future Database Support**: Planned support for PostgreSQL and SQL Server.

## Getting Started

### Prerequisites

- .NET 8 SDK
- MySQL Server

### Installation

1. **Clone the repository**:
   ```sh
   git clone https://github.com/KaustavCodes/JadedCMS-DotNet.git
   cd JadedCms-DotNet
   ```

2. **Set up the database**:
   - Create a new MySQL database.
   - Update the connection string in `appsettings.json` to point to your MySQL database.

3. **Run the application**:
   ```sh
   dotnet run
   ```

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes. Make sure to follow the existing code style and include tests for new features or bug fixes.

## Roadmap

- [ ] Build the CMS Core using MySql as the database
- [ ] Add the global tables and business logic
- [ ] Add frontend page render logic via API's
- [ ] Add SQL Server support
- [ ] Add PostgreSQL support
- [ ] Improve documentation
- [ ] Create example plugins

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For questions or suggestions, feel free to open an issue.

---

Thank you for using JadedCms-DotNet! We hope it makes your content management easier and more efficient.