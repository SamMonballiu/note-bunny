using NoteBunny.BLL.Models;
using System;

namespace NoteBunny.FrontEnd.Wpf.DotNetSix.Context
{
    public static class SelectedNoteContext
    {
        private static Note? _selectedNote;
        public static Note? SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                OnSelectedNoteChanged?.Invoke(value);
            }
        }

        public static Action<Note?>? OnSelectedNoteChanged;
        public static bool HasNote => SelectedNote is not null;
    }
}
