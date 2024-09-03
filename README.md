# Skeddybot

**Skeddybot** is a Telegram bot designed to help users organize their events, appointments, and tasks. It provides functionalities for scheduling reminders, viewing scheduled events, and managing reminders through a simple chat interface. The bot utilizes the Telegram Bot API and is built using .NET 8 with clean architecture principles.

## Project Structure

The project is organized into several key components:

### 1. **Core**

- **Contracts**
  - `ITelegramBotService.cs`: Defines the contract for starting the bot.

- **Handlers**
  - `MessageHandler.cs`: Manages sending messages to users.
  - `UserStateHandler.cs`: Handles user state management and event storage.
  - `ValidationHander.cs`: Validates user input for event messages and schedule times.

- **Helpers**
  - `CommandHandler.cs`: Handles different commands (e.g., `/start`, `/help`, `/add`, `/list`, `/delete`) and manages user interactions.

### 2. **Infrastructure**

- **Configurations**
  - `BotConfiguration.cs`: Holds configuration settings such as the bot token and webhook URL.

- **Logging**
  - `ConsoleLogger.cs`: Provides logging functionality to output messages to the console.

### 3. **Services**

- **TelegramBotService.cs**: Implements `ITelegramBotService` to manage the lifecycle of the bot, handle incoming updates, and delegate command processing.

## Features

- **Command Handling**: 
  - `/start`: Welcomes the user and provides a list of commands.
  - `/help`: Provides a help message with bot functionalities.
  - `/add`: Starts the process of adding a new reminder.
  - `/list`: Lists all upcoming reminders for the user.
  - `/delete`: Allows users to delete specific reminders.

- **Event Management**:
  - Users can add new reminders by providing an event message and schedule time.
  - The bot supports editing and confirming event details via inline keyboard buttons.
  - Events can be deleted from the list of reminders.

- **User State Management**:
  - Tracks user state (e.g., expecting event message or schedule time) to manage interactions effectively.
  - Stores and retrieves event messages and scheduled times.

- **Validation**:
  - Validates event messages to ensure they are not empty.
  - Validates schedule times to ensure they are in the future and properly formatted.

- **Logging**:
  - Logs messages to the console for debugging and tracking purposes.

## Setup and Configuration

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/ammarGamal123/Skeddybot.git
   cd Skeddybot
