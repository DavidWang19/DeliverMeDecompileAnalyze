namespace Utage
{
    using System;

    [Flags]
    public enum UiEventMask
    {
        BeginDrag = 1,
        Cancel = 2,
        Deselect = 4,
        Drag = 8,
        Drop = 0x10,
        EndDrag = 0x20,
        InitializePotentialDrag = 0x40,
        Move = 0x80,
        PointerClick = 0x100,
        PointerDown = 0x200,
        PointerEnter = 0x400,
        PointerExit = 0x800,
        PointerUp = 0x1000,
        Scroll = 0x2000,
        Select = 0x4000,
        Submit = 0x8000,
        UpdateSelected = 0x10000
    }
}

