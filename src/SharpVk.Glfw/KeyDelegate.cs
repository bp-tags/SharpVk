namespace SharpVk.Glfw
{
    /// <summary>
    /// The function signature for keyboard key callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that received the event.
    /// </param>
    /// <param name="key">
    /// The keyboard key that was pressed or released.
    /// </param>
    /// <param name="scanCode">
    /// The system-specific scancode of the key.
    /// </param>
    /// <param name="action">
    /// The input action that occured.
    /// </param>
    /// <param name="modifiers">
    /// Bit field describing which modifier keys were held down.
    /// </param>
    public delegate void KeyDelegate(WindowHandle window, Key key, int scanCode, InputAction action, Modifiers modifiers);
}
