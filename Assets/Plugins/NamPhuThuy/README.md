# Module 
## Design Patterns
### Singleton
SimpleSingleton

GameObjectSingleton
- Cần được khởi tạo từ 1 scene Bootstrap

### Initialization
Initialization:
- Ưu điểm: 
  - Guaranteed Execution Order: By using script execution order, you ensure initializations happen in a specific sequence, avoiding dependencies issues where one script relies on another being initialized first. This is particularly crucial for singletons, which are often accessed early in the application lifecycle.

  - Reduced Boilerplate: If you have many components requiring similar initialization steps (e.g., registering with a manager), centralizing the logic can reduce repetitive code in individual scripts.

  - Improved Code Organization: Keeps initialization logic separate from the core functionality of other scripts, making them easier to read and maintain. It provides a single point of control for all initializations.

  - Easier Debugging: If initialization problems arise, you have a single place to investigate rather than tracking down issues across multiple scripts.

- Nhược điểm: 
  - Tight Coupling: The Initialization.cs script becomes tightly coupled to the specific components it initializes. Changes to those components might require changes to the initialization script as well.

  - Reduced Encapsulation: Components lose control over their own initialization, which can be problematic if they have specific needs or dependencies.

  - Potential for Over-Engineering: For smaller projects or simpler initialization procedures, a dedicated script might be overkill and add unnecessary complexity.

  - Reflection Overhead: Using reflection, as the script does for singletons, can have a slight performance impact, although it's usually negligible. However, it can make code harder to understand and debug.