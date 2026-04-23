mergeInto(LibraryManager.library, {
    UnlockOrientation: function() {
        if (screen.orientation && screen.orientation.unlock) {
            screen.orientation.unlock();
        }
    }
});