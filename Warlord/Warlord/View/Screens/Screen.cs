using System.Collections.Generic;
using Warlord.View.Human.Input;

namespace Warlord.View.Human.Screens
{
    abstract class Screen
    {
        private Stack<ScreenElement> screenElements = new Stack<ScreenElement>();
        private Stack<KeyboardListener> keyboardListeners = new Stack<KeyboardListener>();
        private Stack<MouseListener> mouseListeners = new Stack<MouseListener>();

        protected Screen()
        {
            screenElements = new Stack<ScreenElement>();
            keyboardListeners = new Stack<KeyboardListener>();
            mouseListeners = new Stack<MouseListener>();
        }

        protected void PushScreenElement(ScreenElement element)
        {
            screenElements.Push(element);
        }
        protected ScreenElement PopScreenElement()
        {
            return screenElements.Pop();
        }
        protected void PushKeyboardListener(KeyboardListener element)
        {
            keyboardListeners.Push(element);
        }
        protected KeyboardListener PopKeyboardListener()
        {
            return keyboardListeners.Pop();
        }
        protected void PushMouseListener(MouseListener element)
        {
            mouseListeners.Push(element);
        }
        protected MouseListener PopMouseListener()
        {
            return mouseListeners.Pop();
        }

        public Stack<ScreenElement> ScreenElements
        {
            get { return screenElements; }
        }
        public Stack<KeyboardListener> KeyboardListeners
        {
            get { return keyboardListeners; }
        }
        public Stack<MouseListener> MouseListeners
        {
            get { return mouseListeners; }
        }
    }
}
