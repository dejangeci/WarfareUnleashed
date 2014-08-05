This is a C# port of the first 6 chapters from the book "SFML Game Development".

For the most part, it follows the original source code. These are the differences:

### Features missing in C#:

* sf::Clock -> System.Diagnostics.Stopwatch
* sf::Time -> System.TimeSpan
* sf::Time.asSeconds() -> TimeSpan.Milliseconds / 1000f
* sf::Thread -> System.Threading.Thread
* sf::Mutex -> lock(object)
* std::size_t -> int

### Style:

* Members are defined before methods
* Dropped 'm' prefix for member variables
* All object instances are created within the constructor
* Value members are not initalized with default values, such as bool

### Notes:

* ProcessEvents() usually only calls RenderWindow.DispatchEvents(), C# events are used instead of polling
* ResourceHolder is implemented as a generic class, so there are differences from the original code
* Command is implemented differently - abstract class and Execute method instead of generic class with action delegate
* Chapter 5/6: Application.cs extracts state context in order to register state factories
